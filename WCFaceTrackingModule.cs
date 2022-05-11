using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRCFaceTracking;
using VRCFaceTracking.Params;
using WCFace.Data;

namespace WCFace
{
    public class WCFaceTrackingModule : ExtTrackingModule
    {
        private WCFaceData wcFaceData;
        public override (bool SupportsEye, bool SupportsLip) Supported => (true, true);
        public override (bool UtilizingEye, bool UtilizingLip) Utilizing { get; set; }
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

        public override void Update() { }
    }
}
