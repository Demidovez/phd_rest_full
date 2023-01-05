using Microsoft.AspNetCore.Mvc;
using phd_api.Models;
using System;
using System.Data;
using System.Collections.Generic;
using Uniformance.PHD;
using phd_api.Helpers;

namespace phd_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        [Route("tag_raw_period")]
        public JsonResult GetTagRawPeriod(string tag, string starttime = "", string endtime = "")
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
                        values[i].ToString(),
                        //Math.Round(values[i], 6, MidpointRounding.AwayFromZero),
                        confidences[i], null
                        );
                }

                phd.Dispose();
                return new JsonResult(new TagItem(tag, tagValues));
            } catch(PHDErrorException ex) {
                if(phd != null) {
                    phd.Dispose();
                }
                
                Console.WriteLine("==================== ERROR ======================");
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("===================== END =======================");
                return new JsonResult(""); 
            }
            
        }

        [HttpGet]
        [Route("tag_raw_now")]
        public JsonResult GetTagRawNow(string tag)
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
                phd.server.StartTime = "NOW-3s"; // "NOW-15m";
                phd.server.EndTime = "NOW";     // "NOW";
                
                phd.server.FetchData(MyTag, ref timestamps, ref values, ref confidences);

                int count = timestamps.GetUpperBound(0);

                TagValue[] tagValues = new TagValue[1];

                if (count > 0)
                {
                    tagValues[0] = new TagValue(
                        DateTime.FromOADate(timestamps[count - 1]),
                        values[count - 1].ToString(),
                        //Math.Round(values[count - 1], 6, MidpointRounding.AwayFromZero),
                        confidences[count - 1], null
                        );
                }

                phd.Dispose();
                return new JsonResult(new TagItem(tag, tagValues));
            } catch(PHDErrorException ex) {
                if(phd != null) {
                    phd.Dispose();
                }
                
                Console.WriteLine("==================== ERROR ======================");
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("===================== END =======================");
                return new JsonResult(""); 
            }
            
        }

        [HttpPost]
        [Route("tags_raw_period")]
        public JsonResult PostTagsRawPeriod([FromBody] TagsPeriodRequest requestData)
        {
           PHD_Server phd = null;

            try {
                phd = new PHD_Server();
                phd.server.Sampletype = SAMPLETYPE.Raw;
                phd.server.ReductionType = REDUCTIONTYPE.None;

                Tags MyTags = new Tags();

                // Формируем список PHD тегов
                foreach (String tag in requestData.tags) {
                    MyTags.Add(new Tag(tag));
                }
                
                // Достаем дату из запроса (или используем дефолтную)
                phd.server.StartTime = Helper.DateTimeToDateStr(requestData.starttime);
                phd.server.EndTime = Helper.DateTimeToDateStr(requestData.endtime);
                
                // Выгружаем из PHD
                DataSet dataset = phd.server.FetchRowData(MyTags);

                // Данные записаны в одну таблицу, где новый тег записывается дальше в таблицу
                // 
                List<TagItem> tagItems = new List<TagItem>();
                List<TagValue> tagValues = new List<TagValue>();
                String oldTagname = ""; 

                for (int i = 0; i < dataset.Tables[0].Rows.Count; i++) {
                    DataRow Row = dataset.Tables[0].Rows[i];

                    String tagname = Row["TagName"].ToString();
                    String timestamp = Row["timestamp"].ToString();
                    String value = Row["Value"].ToString();
                    String confidence = Row["Confidence"].ToString();

                    if(oldTagname == "") {
                        oldTagname = tagname;
                    }

                    if(oldTagname != tagname) {
                        tagItems.Add(new TagItem(oldTagname, tagValues.ToArray()));
                        tagValues = new List<TagValue>();  
                        oldTagname = tagname;
                    }

                    tagValues.Add(new TagValue(
                       DateTime.Parse(timestamp),
                        value,
                        short.Parse(confidence),
                        null
                    ));  

                    if(i == dataset.Tables[0].Rows.Count - 1) {
                        tagItems.Add(new TagItem(oldTagname, tagValues.ToArray()));
                    }
                }

                phd.Dispose();
                return new JsonResult(tagItems);
            } catch(PHDErrorException ex) {
                if(phd != null) {
                    phd.Dispose();
                }
                
                Console.WriteLine("==================== ERROR ======================");
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("===================== END =======================");
                return new JsonResult(""); 

           } catch(Exception ex) {
                Console.WriteLine("==================== ERROR ======================");
                Console.WriteLine("{0}", ex);
                Console.WriteLine("===================== END =======================");
                return new JsonResult(""); 
           }
        }

        [HttpGet]
        [Route("tags_info")]
        public JsonResult GetTagsInfo()
        {
            PHD_Server phd = null;


            try
            {
                phd = new PHD_Server();
                phd.server.Sampletype = SAMPLETYPE.Raw;
                phd.server.ReductionType = REDUCTIONTYPE.None;

                // Достаем дату из запроса (или используем дефолтную)
                phd.server.StartTime = "NOW"; // "NOW-15m";
                phd.server.EndTime = "NOW";     // "NOW";

                // Данные записаны в одну таблицу, где новый тег записывается дальше в таблицу
                // 
                List<TagInfo> tagItems = new List<TagInfo>();
                TagValue[] tagValues = new TagValue[1];

                TagFilter tagFilter = new TagFilter();
                //tagFilter.Tagname = "*2ALIC0117.PIDA.OP*2ALIC0123.PIDA.OP*"; // || ";|2ALIC0123.PIDA.OP
                DataSet tags = phd.server.BrowsingTags(550000, tagFilter);


                for (int i = 0; i < tags.Tables[0].Rows.Count; i++)
                {
                    DataRow Row = tags.Tables[0].Rows[i];

                    String tagname = Row[0].ToString();
                    String tagnumber = Row[1].ToString();
                    String desc = Row[2].ToString();
                    String unit = Row[3].ToString();

                    tagItems.Add(new TagInfo(tagname, tagnumber, desc, unit));
                }

                phd.Dispose();
                return new JsonResult(tagItems);
            }
            catch (PHDErrorException ex)
            {
                if (phd != null)
                {
                    phd.Dispose();
                }

                Console.WriteLine("==================== ERROR ======================");
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("===================== END =======================");
                return new JsonResult("");

            }
            catch (Exception ex)
            {
                Console.WriteLine("==================== ERROR ======================");
                Console.WriteLine("{0}", ex);
                Console.WriteLine("===================== END =======================");
                return new JsonResult("");
            }
        }

        [HttpPost]
        [Route("tags_raw_now")]
        public JsonResult PostTagsRaw([FromBody] TagsRequest requestData)
        {
           PHD_Server phd = null;
            

            try {
                phd = new PHD_Server();
                phd.server.Sampletype = SAMPLETYPE.Raw;
                phd.server.ReductionType = REDUCTIONTYPE.None;

                Tags MyTags = new Tags();

                // Формируем список PHD тегов
                foreach (String tag in requestData.tags) {
                    MyTags.Add(new Tag(tag));
                }
                
                // Достаем дату из запроса (или используем дефолтную)
                phd.server.StartTime = "NOW"; // "NOW-15m";
                phd.server.EndTime = "NOW";     // "NOW";
                
                // Выгружаем из PHD
                DataSet dataset = phd.server.FetchRowData(MyTags);

                // Данные записаны в одну таблицу, где новый тег записывается дальше в таблицу
                // 
                List<TagItem> tagItems = new List<TagItem>();
                TagValue[] tagValues = new TagValue[1];
                String oldTagname = "";


                for (int i = 0; i < dataset.Tables[0].Rows.Count; i++) {
                    DataRow Row = dataset.Tables[0].Rows[i];

                    String tagname = Row["TagName"].ToString();
                    String timestamp = Row["timestamp"].ToString();
                    String value = Row["Value"].ToString();
                    String confidence = Row["Confidence"].ToString();
                    String units = Row["Units"].ToString();

                    if (oldTagname == "") {
                        oldTagname = tagname;
                    }

                    if(oldTagname != tagname) {
                        tagItems.Add(new TagItem(oldTagname, tagValues));
                        tagValues = new TagValue[1];  
                        oldTagname = tagname;
                    }


                    // Console.WriteLine("timestamp: {0} :: {1}", tagname, timestamp);

                    if(timestamp != "" && confidence != "" && value != "") {
                        tagValues[0] = new TagValue(
                            DateTime.Parse(timestamp),
                            value,
                            short.Parse(confidence),
                            units
                        );  
                    }
                    

                    if(i == dataset.Tables[0].Rows.Count - 1) {
                        tagItems.Add(new TagItem(oldTagname, tagValues));
                    }
                }

                phd.Dispose();
                return new JsonResult(tagItems);
            } catch(PHDErrorException ex) {
                if(phd != null) {
                    phd.Dispose();
                }
                
                Console.WriteLine("==================== ERROR ======================");
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("===================== END =======================");
                return new JsonResult(""); 

           } catch(Exception ex) {
                Console.WriteLine("==================== ERROR ======================");
                Console.WriteLine("{0}", ex);
                Console.WriteLine("===================== END =======================");
                return new JsonResult(""); 
           }
        }
    }
}
