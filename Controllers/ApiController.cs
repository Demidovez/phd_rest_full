using Microsoft.AspNetCore.Mvc;
using phd_api.Models;
using System;
using Uniformance.PHD;
using phd_api.Helpers;

namespace phd_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        [Route("get_tag_raw")]
        public JsonResult Get(string tag, string starttime = "", string endtime = "")
        {
            PHD_Server phd = null;

            try {
                phd = new PHD_Server();
                phd.server.Sampletype = SAMPLETYPE.Raw;
                phd.server.ReductionType = REDUCTIONTYPE.None;
            
                Double[] timestamps = null;
                Double[] values = null;
                short[] confidences = null;

                Tag MyTag = new Tag(tag);
                phd.server.StartTime = starttime != "" ? Helper.StrtTimestampToDateStr(starttime) : "NOW-1m"; // "NOW-15m";
                phd.server.EndTime = endtime != "" ? Helper.StrtTimestampToDateStr(endtime) : "NOW";     // "NOW";
                
                phd.server.FetchData(MyTag, ref timestamps, ref values, ref confidences);

                int count = timestamps.GetUpperBound(0);

                TagValue[] tagValues = new TagValue[count];

                for (int i = 0; i < count; ++i)
                {
                    tagValues[i] = new TagValue(
                        DateTime.FromOADate(timestamps[i]), 
                        Math.Round(values[i], 6, MidpointRounding.AwayFromZero),
                        confidences[i]
                        );
                }

                phd.Dispose();
                return new JsonResult(new TagItem(tag, tagValues));
            } catch(Exception e) {
                if(phd != null) {
                    phd.Dispose();
                }
                
                Console.WriteLine("{0} Exception caught.", e);
                return new JsonResult(""); 
            }
            
        }
    }
}
