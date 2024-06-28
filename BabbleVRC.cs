using System.Reflection;

namespace VRCFaceTracking.Babble;

public class BabbleVRC : ExtTrackingModule
{
	private BabbleOSC babbleOSC;

	public override (bool SupportsEye, bool SupportsExpression) Supported => (false, true);

	public override (bool eyeSuccess, bool expressionSuccess) Initialize(bool eyeAvailable, bool expressionAvailable)
	{
		babbleOSC = new BabbleOSC(Logger);

		var streams = new List<Stream>();
		var a = Assembly.GetExecutingAssembly();
		var hmdStream = a.GetManifestResourceStream
			("VRCFaceTracking.Babble.BabbleLogo.png");
		streams.Add(hmdStream!);
		ModuleInformation = new ModuleMetadata
		{
			Name = "Project Babble Face Tracking\nInference Model v2.0.7",
			StaticImages = streams
		};

		return (false, true);
	}

	public override void Teardown()
	{
		babbleOSC.Teardown();
	}

	public override void Update()
	{
		foreach (var unifiedExpression in BabbleExpressions.BabbleExpressionMap)
			UnifiedTracking.Data.Shapes[(int)unifiedExpression].Weight =
				BabbleExpressions.BabbleExpressionMap.GetByKey1(unifiedExpression);
		Thread.Sleep(10);
	}
}