using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

// initial source: https://www.youtube.com/watch?v=FcnvwtyxLds
public class Rope : MonoBehaviour
{
    public static Color GIZMOS_COLOR = new Color(1, 0, 0.7f);

    private List<RopeSegment> ropeSegments = new List<RopeSegment>();

    [Header("Editor Settings", order = 1)]
    [Space(-10, order = 2)]
    [Header("This may NOT be simulated", order = 3)]
    [Space(-10, order = 4)]
    [Header("accurately in editor", order = 5)]
    public bool useInEditor = false;

    [Header("Settings")]
    [SerializeField] private bool useEndPoint = false;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField][Min(3)] private int segmentCount = 35;
    [SerializeField] private float ropeLength = 10;
    [SerializeField][Min(1)] private int simulationNumber = 30;
    [SerializeField] private Vector2 forceGravity = new Vector2(0f, -1f);

    private float RopeSegLen
    {
        get
        {
            return ropeLength / segmentCount;
        }
    }

    [Header("Extensions")]
    [SerializeField] private ExtensionWind extensionWind;
    [SerializeField] private ExtensionAffector extensionAffector;
    [SerializeField] private ExtensionCollider extensionCollider;
    [SerializeField] private ExtensionCustomAtEndPoint extensionCustomAtEndPoint;

    [Header("Performance")]
    [SerializeField] private bool outOfPlayerRangeStopSimulation = true;

    [Header("Requirements")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform atRopeStart;
    [SerializeField] private Transform atRopeEnd;

    private Vector3 prevStartPoint = Vector3.zero;
    private Vector3 prevEndPoint = Vector3.zero;

    private PlayerRopeProfile playerRopeProfile;

    void Awake()
    {
        useInEditor = false;
    }

    void Start()
    {
        LoadRopeSegments();
        // Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Setup();
        playerRopeProfile = PlayerRopeProfile.instance;
    }

    void Setup()
    {
        if (ropeSegments.Count == segmentCount) return;

        ropeSegments = new List<RopeSegment>();
        Vector3 ropeStartPoint = startPoint.position;

        for (int i = 0; i < segmentCount; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint += (Vector3)forceGravity.normalized * RopeSegLen;
        }
    }

    void LoadRopeSegments()
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        ropeSegments = new List<RopeSegment>();

        foreach (var pos in positions)
        {
            ropeSegments.Add(new RopeSegment(pos));
        }
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (Application.isPlaying)
        {
            if (outOfPlayerRangeStopSimulation && !IsInPlayerRange())
                return;
        }

        /*
        if (prevStartPoint != startPoint.position ||
            prevEndPoint != endPoint.position
        )
        {
            Simulate();
            prevStartPoint = startPoint.position;
            prevEndPoint = endPoint.position;
        }
        */
        Simulate();

        DrawRope();

        if (extensionWind.enabled)
            extensionWind.FixedUpdate();

        FollowerUpdate();
    }

    public void EditorUpdate()
    {
        Setup();
        // Simulate();
        FixedUpdate();
    }

    private void FollowerUpdate()
    {
        atRopeStart.transform.position = ropeSegments[0].posNow;

        int index = ropeSegments.Count - 1;
        if (extensionCustomAtEndPoint.enabled)
        {
            index = extensionCustomAtEndPoint.GetAtEndPointIndex(ropeSegments.Count);
            index--;
            if (index <= 0) index = 1;
        }

        atRopeEnd.transform.position = ropeSegments[index].posNow;
        float _angle = Vector2.SignedAngle(Vector2.up, ropeSegments[index].posNow - ropeSegments[index - 1].posNow);
        //_angle *= Mathf.Rad2Deg;
        atRopeEnd.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
    }

    private bool IsInPlayerRange()
    {
        // check for both dst between start point and end point to player profile
        if (playerRopeProfile.IsInArea(startPoint.position) || playerRopeProfile.IsInArea(endPoint.position))
            return true;

        return false;
    }

    private void Simulate()
    {
        // SIMULATION
        for (int i = 0; i < segmentCount; i++)
        {
            RopeSegment thisSeg = ropeSegments[i];

            // EXTENSION
            if (extensionCollider.enabled && extensionCollider.CheckCollision(thisSeg.posNow))
                thisSeg.posNow = thisSeg.posOld;

            Vector2 velocity = thisSeg.posNow - thisSeg.posOld;
            thisSeg.posOld = thisSeg.posNow;

            velocity += forceGravity * Time.deltaTime;

            // EXTENSIONS
            if (extensionWind.enabled)
            {
                Vector2 forceWind = extensionWind.GetForceWind();
                velocity += forceWind * Time.deltaTime;
            }
            if (extensionAffector.enabled && Application.isPlaying)
            {
                Vector2 forceAffector = extensionAffector.GetForceAffector(thisSeg.posNow);
                velocity += forceAffector * Time.deltaTime;
            }

            thisSeg.posNow += velocity;
            ropeSegments[i] = thisSeg;
        }

        // CONSTRAINTS
        for (int i = 0; i < simulationNumber; i++)
        {
            ApplyConstraints();
        }
    }

    private void ApplyConstraints()
    {
        // start and end point constraints
        RopeSegment firstSeg = ropeSegments[0];
        firstSeg.posNow = startPoint.position;
        ropeSegments[0] = firstSeg;

        if (useEndPoint)
        {
            RopeSegment lastSeg = ropeSegments[ropeSegments.Count - 1];
            lastSeg.posNow = endPoint.position;
            ropeSegments[ropeSegments.Count - 1] = lastSeg;
        }

        // same length constraint
        for (int i = 0; i < segmentCount - 1; i++)
        {
            RopeSegment thisSeg = this.ropeSegments[i];
            RopeSegment nextSeg = this.ropeSegments[i + 1];

            float dist = (thisSeg.posNow - nextSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - RopeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > RopeSegLen)
            {
                changeDir = (thisSeg.posNow - nextSeg.posNow).normalized;
            }
            else if (dist < RopeSegLen)
            {
                changeDir = (nextSeg.posNow - thisSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;

            if (i == 0)
            {
                // first segment
                nextSeg.posNow += changeAmount;
                ropeSegments[i + 1] = nextSeg;
            }
            else if (useEndPoint && i == segmentCount - 2)
            {
                // last segment
                thisSeg.posNow -= changeAmount;
                ropeSegments[i] = thisSeg;
            }
            else
            {
                thisSeg.posNow -= changeAmount * 0.5f;
                ropeSegments[i] = thisSeg;
                nextSeg.posNow += changeAmount * 0.5f;
                ropeSegments[i + 1] = nextSeg;
            }
        }
    }

    private void DrawRope()
    {
        float _lineWidth = lineWidth;
        lineRenderer.startWidth = _lineWidth;
        lineRenderer.endWidth = _lineWidth;

        Vector3[] ropePositions = new Vector3[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }

        public RopeSegment(Vector2 posNow, Vector2 posOld)
        {
            this.posNow = posNow;
            this.posOld = posOld;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = GIZMOS_COLOR;

        Gizmos.DrawLine(startPoint.position, endPoint.position);

        if (extensionWind.enabled)
        {
            extensionWind.GizmosDrawWindDir(startPoint.position);
        }
    }

    [System.Serializable]
    public class ExtensionWind
    {
        [Header("This may NOT be simulated", order = 1)]
        [Space(-10, order = 2)]
        [Header("accurately in editor", order = 3)]
        public bool enabled;
        public Vector2 direction = Vector2.right;
        public float magnitude = 1;
        private Vector2 noisePos = Vector2.zero;
        public Vector2 magFlucFac = new Vector2(1, 1);
        public float flucSpd = 1;

        public void FixedUpdate()
        {
            noisePos += direction.normalized * flucSpd * Time.fixedDeltaTime;
        }

        public Vector2 GetForceWind()
        {
            float _perlin = Mathf.PerlinNoise(noisePos.x, noisePos.y);
            return direction.normalized * magnitude * Mathf.Lerp(magFlucFac.x, magFlucFac.y, _perlin);
        }

        public void GizmosDrawWindDir(Vector2 _pos)
        {
            var _angle = Vector2.SignedAngle(Vector2.up, direction);
            Vector2 _arrow1 = new Vector2(0, 0.3f);
            Vector2 _arrow2 = new Vector2(0, 0.0f);
            Vector2 _arrow3 = new Vector2(0, -0.3f);
            _arrow1 = RotatePointAroundZero(_arrow1, _angle);
            _arrow2 = RotatePointAroundZero(_arrow2, _angle);
            _arrow3 = RotatePointAroundZero(_arrow3, _angle);

            GizmosDrawArrow(_pos + _arrow1, _angle);
            GizmosDrawArrow(_pos + _arrow2, _angle);
            GizmosDrawArrow(_pos + _arrow3, _angle);
        }

        private void GizmosDrawArrow(Vector2 _pos, float _angle)
        {
            Vector2 _pos1 = new Vector2(-1f, -0.5f) * 0.3f;
            Vector2 _pos2 = new Vector2(0f, .5f) * 0.3f;
            Vector2 _pos3 = new Vector2(1f, -0.5f) * 0.3f;

            Vector2 _axis = new Vector2(0, 0);

            _pos1 = RotatePointAroundZero(_pos1, _angle);
            _pos2 = RotatePointAroundZero(_pos2, _angle);
            _pos3 = RotatePointAroundZero(_pos3, _angle);

            Gizmos.DrawLine(_pos + _pos1, _pos + _pos2);
            Gizmos.DrawLine(_pos + _pos2, _pos + _pos3);
        }

        Vector2 RotatePointAroundZero(Vector2 _point, float _angle)
        {
            _angle = _angle * Mathf.Deg2Rad;
            float _cos = Mathf.Cos(_angle);
            float _sin = Mathf.Sin(_angle);
            float _x = _point.x * _cos - _point.y * _sin;
            float _y = _point.x * _sin + _point.y * _cos;
            return new Vector2(_x, _y);
        }
    }

    [System.Serializable]
    public class ExtensionAffector
    {
        public bool enabled;
        public float magnitude;

        public Vector2 GetForceAffector(Vector2 _pos)
        {
            Vector2 _force = RopeAffector.GetForceAffector(_pos);
            return _force * magnitude;
        }
    }

    [System.Serializable]
    public class ExtensionCollider
    {
        public bool enabled;
        public LayerMask layerMask;

        public bool CheckCollision(Vector2 _point)
        {
            Collider2D _collider = Physics2D.OverlapPoint(_point, layerMask);
            if (_collider) return true;
            return false;
        }
    }

    [System.Serializable]
    public class ExtensionCustomAtEndPoint
    {
        public bool enabled;
        [Range(0, 1)]
        public float atEndPointRatio = 1;

        public int GetAtEndPointIndex(int segmentCount)
        {
            return Mathf.FloorToInt(Mathf.Lerp(0, segmentCount, atEndPointRatio));
        }
    }
}

