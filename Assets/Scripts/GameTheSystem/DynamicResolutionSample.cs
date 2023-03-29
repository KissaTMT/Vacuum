using System.Collections;
using UnityEngine;

public class DynamicResolutionSample : MonoBehaviour
{
    /// <summary>
    /// Tells if the text has to be visible (true) or not (false).
    /// </summary>
    /// <summary>
    /// Disables everything when true.
    /// </summary>
    //public bool DisableResolutionChangesAndEvaluation = false;
    /// <summary>
    /// Overrides the default resolution (1 means normal, 0.9f is 90%, and so on)
    /// </summary>
    public float StartingRatioOverride;
    /// <summary>
    /// Setting to say that the current scene has static resolution (basically disables the script except for StartingRatioOverride)
    /// </summary>
    //public bool StaticResolution = false;
    /// <summary>
    /// Setting to decide if the resolution has to be changed based on scene average fps or based on instant fps.
    /// </summary>
    //public bool BasedOnSceneAvg = true;
    /// <summary>
    /// Setting to decide how many times a scene can chenge the resolution
    /// 0 = false; -1 = true (until minimum); 
    /// 1 means 1 time, 2 means 2 times and so on.
    /// </summary>
    public int InvokeRecursively; // 0 = false; -1 = true; 1 means 1 time, 2 means 2 times and so on.
    /// <summary>
    /// Setting to tell if the resolution has to be changed at screen change or as soon as possible. 
    /// Enabled by default since resolution changes are visible. 
    /// Still changing all the time (false) can be useful if you have a scene dedicate to hardware estimation.
    /// </summary>
    //public bool OnlyOnSceneChange = true;
    /// <summary>
    /// Resolution will go lower if FPS (or average FPS ot the scene, depending on settings) drop below this value
    /// </summary>
    public int InferiorFpsLimit;
    /// <summary>
    /// Resolution will go higher if FPS (or average FPS ot the scene, depending on settings) goes above this value
    /// </summary>
    public int SuperiorFpsLimit;
    /// <summary>
    /// Sets the maximum FPS of the scene (60 is usually the maximum allowed on smartphones)
    /// </summary>
    public int TargetFps;

    private const float startingPeriod = 0.5f;
    private const float regimePeriod = 2.0f;
    private const float avgFpsStartPeriod = 2.0f; // better be a multiple of regimePeriod
    
    private static int lastSceneAvgFps = 0;
    private static float fpsMeasurePeriod;
    private static int originalwidth;
    private static int originalheight;
    private static int currentwidth;
    private static int currentheight;

    private bool _startAvg = false;

    private int _fpsAccumulator;
    private float _fpsNextPeriod;
    private int _currentFps;
    private int _avgFpsAccumulator;
    private int _currentAvgFps;

    private int _minimalwidth;
    private int _minimalheight;

    public static bool resized { get; private set; }

    private void Update()
    {
        // measuring frames per second in the fpsMeasurePeriod
        _fpsAccumulator++;
        _avgFpsAccumulator++;

        if (Time.realtimeSinceStartup > _fpsNextPeriod)
        {
            var appFps = _currentFps;
            _currentFps = (int)(_fpsAccumulator / fpsMeasurePeriod);
            if (appFps > _currentFps) _startAvg = true;
            _fpsAccumulator = 0;
            _fpsNextPeriod += fpsMeasurePeriod;

            //Debug.Log("Time Since Level Load " + Time.timeSinceLevelLoad);
            if (_startAvg && Time.timeSinceLevelLoad > avgFpsStartPeriod)
            {
                _currentAvgFps = (int)(_avgFpsAccumulator / Time.timeSinceLevelLoad);
                lastSceneAvgFps = _currentAvgFps;
            }
            else
            {
                //Debug.Log("Not yet started recording avg fps");
                _currentAvgFps = _currentFps;
                _avgFpsAccumulator = (int)(_currentAvgFps * Time.timeSinceLevelLoad);
            }
        }
    }
    private void Start()
    {
        // initialization

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFps;

        resized = resized ? true : false;
        fpsMeasurePeriod = startingPeriod;

        originalwidth = (originalwidth == 0) ? Screen.width : originalwidth;
        originalheight = (originalheight == 0) ? Screen.height : originalheight;

        currentwidth = (currentwidth == 0) ? originalwidth : currentwidth;
        currentheight = (currentheight == 0) ? originalheight : currentheight;

        _minimalwidth = (int)(originalwidth * 0.69f);
        _minimalheight = (int)(originalheight * 0.69f);

        var ratioOverride = (!resized) ? StartingRatioOverride : 1;

        _fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;

        // decisions
        if (ratioOverride != 0.9f && !resized) Resize((int)(currentwidth * ratioOverride), (int)(currentheight * ratioOverride));
        else if (resized)
        {
            Resize(currentwidth, currentheight);
            lastSceneAvgFps = 0;
        }
        if (InvokeRecursively == -1 || InvokeRecursively > 0) Invoke(nameof(RefreshResolution), regimePeriod);
    }
    private void Resize(int width, int height)
    {
        //Debug.Log(string.Format("resizing to {0}x{1}", width, height));
        Screen.SetResolution(width, height, true, TargetFps);
        resized = true;
    }

    private void RefreshResolution()
    {
        // bases resolution change on the selected option, so scene average or current fps
        var fpsToUse = (float)(lastSceneAvgFps > 0 ? lastSceneAvgFps : _currentAvgFps);
        var changed = false;
        if (fpsToUse < InferiorFpsLimit && fpsToUse > 0 && currentheight > _minimalheight)
        {
            var ratio = resized ? 0.92f : 0.82f;
            currentheight = (int)(currentheight * ratio);
            if (currentheight < _minimalheight) currentheight = _minimalheight;
            currentwidth = (int)(currentwidth * ratio);
            if (currentwidth < _minimalwidth) currentwidth = _minimalwidth;

            // if it's realtime dynamic resolution applies the resize
            Resize(currentwidth, currentheight);
            changed = true;
        }
        else if (resized && fpsToUse >= SuperiorFpsLimit && currentheight < originalheight)
        {
            currentheight = (int)(currentheight * 1.1f);
            if (currentheight > originalheight) currentheight = originalheight;
            currentwidth = (int)(currentwidth * 1.1f);
            if (currentwidth > originalwidth) currentwidth = originalwidth;

            // if it's realtime dynamic resolution applies the resize
            Resize(currentwidth, currentheight);
            changed = true;
        }

        resized = true;

        if (InvokeRecursively > 0 && changed)
        {
            InvokeRecursively--;
            changed = false;
        }
        if (InvokeRecursively == -1 || InvokeRecursively > 0) Invoke(nameof(RefreshResolution),regimePeriod);
    }
}