using System;
using System.Threading;
using VRCFaceTracking;
using WCFace.Data;

namespace WCFace
{
    public class WCFaceTrackingModule : ExtTrackingModule
    {
        private WCFaceData wcFaceData;
        public override (bool SupportsEye, bool SupportsLip) Supported => (true, true);
        public override Action GetUpdateThreadFunc() { return () => { }; }

        public override (bool eyeSuccess, bool lipSuccess) Initialize(bool eye, bool lip)
        {
            if (wcFaceData is null)
            {
                wcFaceData = new WCFaceData();
            }

            wcFaceData.Initialize(true, true);
            wcFaceData.StartThread();

            Logger.Msg("WCFaceTracking initialized! The first few packets might throw some errors, this is fine.");
            Logger.Msg("Refer to the WCFace Console for Tracking Info.");
            Thread.Sleep(2500);

            return (true, true);
        }

        public override void Teardown()
        {
            wcFaceData.Teardown();
        }
    }
}
