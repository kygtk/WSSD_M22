using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Data
{
    public enum RecipeItem
    {
        V, // Current Recipe flag
        Id,
        ChangedTime,
        Comment,
        TactTime,
        CvSpeed,
        //ApUse, 
        EUVLAMP1_USE,
        EUVLAMP2_USE,
        EUVLAMP3_USE,
        //ApVoltage, 
        //ApN2Flow,
        //ApCDAFlow,
        RB1UpUse,
        RB1LoUse,
        RB2UpUse,
        RB2LoUse,
        RB1UpDir,
        RB1LoDir,
        RB2UpDir,
        RB2LoDir,
        RB1UpSpeed,
        RB1LoSpeed,
        RB2UpSpeed,
        RB2LoSpeed,
        RB1UpGap,
        RB1LoGap,
        RB2UpGap,
        RB2LoGap,
        MjUse,
        HpmjUse,
        HpmjPressure,
        //TrAlignUse,
        MaxItemCount
    }

    //public class RecipeItemInfo // 10.12.25 minhan
    //{
    //    public static int StartIndex = 1; // ID 부터, V==0
    //    public static int MaxCount = (int)RecipeItem.MaxItemCount - StartIndex;

    //    public static string[] Names = new string[] 
    //    { 
    //        "USING", 
    //        "ID", 
    //        "CHANGE_TIME", 
    //        "VERSION",
    //        "TACT_TIME",
    //        "CONV_SPD",
    //        "AP_USE",
    //        "AP_PWR",
    //        "AP_N2_FLW",
    //        "AP_CDA_FLW",
    //        "RB1_U_USE",
    //        "RB1_L_USE",
    //        "RB2_U_USE",
    //        "RB2_L_USE",
    //        "RB1_U_DIR",
    //        "RB1_L_DIR",
    //        "RB2_U_DIR",
    //        "RB2_L_DIR",
    //        "RB1_U_SPD",
    //        "RB1_L_SPD",
    //        "RB2_U_SPD",
    //        "RB2_L_SPD",
    //        "RB1_U_GAP",
    //        "RB1_L_GAP",
    //        "RB2_U_GAP",
    //        "RB2_L_GAP",
    //        "MJ_USE",
    //        "FR_HP_USE",
    //        "FR_HP_PRS",
    //        "TR_ALIGN_USE"
    //    };
    //}

    [Serializable()]
    public struct TagRecipe
    {
        public string Id;
        public DateTime ChangedTime;
        public string Comment;
        public int TactTime;
        public int CvSpeed;
        public bool EUVLAMP1_USE;
        public bool EUVLAMP2_USE;
        public bool EUVLAMP3_USE;
        //public bool ApUse;
        //public double ApVoltage;
        //public double ApN2Flow;
        //public double ApCDAFlow;
        public bool RB1UpUse;
        public bool RB1LoUse;
        public bool RB2UpUse;
        public bool RB2LoUse;
        public bool RB1UpDir;
        public bool RB1LoDir;
        public bool RB2UpDir;
        public bool RB2LoDir;
        public int RB1UpSpeed;
        public int RB1LoSpeed;
        public int RB2UpSpeed;
        public int RB2LoSpeed;
        public double RB1UpGap;
        public double RB1LoGap;
        public double RB2UpGap;
        public double RB2LoGap;
        public bool MjUse;
        public bool HpmjUse;
        public int HpmjPressure;
        //public bool TrAlignUse; // 11.02.09 minhan

        public TagRecipe(string recipeId) // 11.02.17 minhan
        {
            Id = recipeId;
            ChangedTime = DateTime.Now;
            Comment = "New";
            TactTime = 45;
            CvSpeed = 4320;
            EUVLAMP1_USE = true;
            EUVLAMP2_USE = true;
            EUVLAMP3_USE = true;
            //ApUse = true;
            //ApVoltage = 12.0; // 11.04.20 minhan
            //ApN2Flow = 1200;
            //ApCDAFlow = 7.2;
            RB1UpUse = true;
            RB1LoUse = true;
            RB2UpUse = true;
            RB2LoUse = true;
            RB1UpDir = true;
            RB1LoDir = false;
            RB2UpDir = false;
            RB2LoDir = true;
            RB1UpSpeed = 400;
            RB1LoSpeed = 400;
            RB2UpSpeed = 400;
            RB2LoSpeed = 400;
            RB1UpGap = 0.25;
            RB1LoGap = 0.25;
            RB2UpGap = 0.25;
            RB2LoGap = 0.25;
            MjUse = true;
            HpmjUse = true;
            HpmjPressure = 100;
            //TrAlignUse = true;
        }

        public void Clone(TagRecipe item)
        {
            Id = item.Id;
            ChangedTime = item.ChangedTime;
            Comment = item.Comment;
            TactTime = item.TactTime;
            CvSpeed = item.CvSpeed;
            EUVLAMP1_USE = item.EUVLAMP1_USE;
            EUVLAMP2_USE = item.EUVLAMP2_USE;
            EUVLAMP3_USE = item.EUVLAMP3_USE;
            //ApUse = item.ApUse;
            //ApVoltage = item.ApVoltage;
            //ApN2Flow = item.ApN2Flow;
            //ApCDAFlow = item.ApCDAFlow;
            RB1UpUse = item.RB1UpUse;
            RB1LoUse = item.RB1LoUse;
            RB2UpUse = item.RB2UpUse;
            RB2LoUse = item.RB2LoUse;
            RB1UpDir = item.RB1UpDir;
            RB1LoDir = item.RB1LoDir;
            RB2UpDir = item.RB2UpDir;
            RB2LoDir = item.RB2LoDir;
            RB1UpSpeed = item.RB1UpSpeed;
            RB1LoSpeed = item.RB1LoSpeed;
            RB2UpSpeed = item.RB2UpSpeed;
            RB2LoSpeed = item.RB2LoSpeed;
            RB1UpGap = item.RB1UpGap;
            RB1LoGap = item.RB1LoGap;
            RB2UpGap = item.RB2UpGap;
            RB2LoGap = item.RB2LoGap;
            MjUse = item.MjUse;
            HpmjUse = item.HpmjUse;
            HpmjPressure = item.HpmjPressure;
            //TrAlignUse = item.TrAlignUse;
        }

        //public void ConverToStream(ref ushort[] stream, int startIndex, int count) // 10.12.25 minhan
        //{
        //    if (stream != null)
        //    {
        //        //if ((stream.Length - startIndex) < count)
        //        //{
        //        //    return;
        //        //}

        //        int YY = (ushort)XFunc.ConvertIntToBcd(this.ChangedTime.Year - 2000);
        //        int MM = (ushort)XFunc.ConvertIntToBcd(this.ChangedTime.Month);
        //        int DD = (ushort)XFunc.ConvertIntToBcd(this.ChangedTime.Day);
        //        int hh = (ushort)XFunc.ConvertIntToBcd(this.ChangedTime.Hour);
        //        int mm = (ushort)XFunc.ConvertIntToBcd(this.ChangedTime.Minute);
        //        int ss = (ushort)XFunc.ConvertIntToBcd(this.ChangedTime.Second);
        //        int i = startIndex;
        //        stream[i++] = (ushort)(YY << 8 | MM);
        //        stream[i++] = (ushort)(DD << 8 | hh);
        //        stream[i++] = (ushort)(mm << 8 | ss);
        //        stream[i++] = (ushort)(1); //version
        //        stream[i++] = (ushort)(2); //version
        //        stream[i++] = (ushort)(3); //version
        //        stream[i++] = (ushort)(this.TactTime);
        //        stream[i++] = (ushort)(this.CvSpeed);
        //        stream[i++] = (ushort)(this.ApUse ? 1 : 0);
        //        stream[i++] = (ushort)(this.ApVoltage * 10.0);
        //        stream[i++] = (ushort)(this.ApN2Flow * 10.0);
        //        stream[i++] = (ushort)(this.ApCDAFlow * 10.0);
        //        stream[i++] = (ushort)(this.RB1UpUse ? 1 : 0);
        //        stream[i++] = (ushort)(this.RB1LoUse ? 1 : 0);
        //        stream[i++] = (ushort)(this.RB2UpUse ? 1 : 0);
        //        stream[i++] = (ushort)(this.RB2LoUse ? 1 : 0);
        //        stream[i++] = (ushort)(this.RB1UpDir ? 1 : 0);
        //        stream[i++] = (ushort)(this.RB1LoDir ? 1 : 0);
        //        stream[i++] = (ushort)(this.RB2UpDir ? 1 : 0);
        //        stream[i++] = (ushort)(this.RB2LoDir ? 1 : 0);
        //        stream[i++] = (ushort)(this.RB1UpSpeed);
        //        stream[i++] = (ushort)(this.RB1LoSpeed);
        //        stream[i++] = (ushort)(this.RB2UpSpeed);
        //        stream[i++] = (ushort)(this.RB2LoSpeed);
        //        stream[i++] = (ushort)(this.RB1UpGap * 10.0);
        //        stream[i++] = (ushort)(this.RB1LoGap * 10.0);
        //        stream[i++] = (ushort)(this.RB2UpGap * 10.0);
        //        stream[i++] = (ushort)(this.RB2LoGap * 10.0);
        //        stream[i++] = (ushort)(this.MjUse ? 1 : 0);
        //        stream[i++] = (ushort)(this.HpmjUse ? 1 : 0);
        //        stream[i++] = (ushort)(this.HpmjPressure * 10.0);
        //        stream[i++] = (ushort)(this.TrAlignUse ? 1 : 0);
        //    }
        //}

        //public void ConvertToRecipe(short[] stream) // 10.12.25 minhan
        //{
        //    int i = 0;
        //    string yymm = string.Format("{0:d4}", XFunc.ConvertBcdToInt(stream[i++]));
        //    string ddhh = string.Format("{0:d4}", XFunc.ConvertBcdToInt(stream[i++]));
        //    string mmss = string.Format("{0:d4}", XFunc.ConvertBcdToInt(stream[i++]));

        //    string date = string.Format("20{0}-{1}-{2} {3}:{4}:{5}", yymm.Substring(0, 2), yymm.Substring(2, 2), ddhh.Substring(0, 2), ddhh.Substring(2, 2), mmss.Substring(0, 2), mmss.Substring(2, 2));
        //    ChangedTime = DateTime.ParseExact(date, "yyyy-MM-dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        //    Comment = XFunc.ConvertToString(stream, i, 3, ByteOrder.BigEndian);
        //    TactTime = stream[3 + i++];
        //    CvSpeed = stream[i++];
        //    ApUse = (stream[i++] == 1) ;
        //    ApVoltage = stream[i++] / 10;
        //    ApN2Flow = stream[i++] / 10;
        //    ApCDAFlow = stream[i++] / 10;
        //    RB1UpUse = (stream[i++] == 1) ;
        //    RB1LoUse = (stream[i++] == 1) ;
        //    RB2UpUse = (stream[i++] == 1) ;
        //    RB2LoUse = (stream[i++] == 1) ;
        //    RB1UpDir = (stream[i++] == 1) ;
        //    RB1LoDir = (stream[i++] == 1) ;
        //    RB2UpDir = (stream[i++] == 1) ;
        //    RB2LoDir = (stream[i++] == 1) ;
        //    RB1UpSpeed = stream[i++];
        //    RB1LoSpeed = stream[i++];
        //    RB2UpSpeed = stream[i++];
        //    RB2LoSpeed = stream[i++];
        //    RB1UpGap = stream[i++] / 10;
        //    RB1LoGap = stream[i++] / 10;
        //    RB2UpGap = stream[i++] / 10;
        //    RB2LoGap = stream[i++] / 10;
        //    MjUse = (stream[i++] == 1) ;
        //    HpmjUse = (stream[i++] == 1) ;
        //    HpmjPressure = stream[i++] / 10;
        //    TrAlignUse = (stream[i++] == 1) ;
        //}

        public static bool IsAvailable(short[] data)
        {
            bool ng = false;
            return !ng;
        }

        public static bool IsAvailableRecipeId(int id)
        {
            return id <= 99 && id >= 1;
        }

        public static bool CheckValidation(int itemIndex, string value) // 10.12.25 minhan TryParse 적용.
        {
            // check itme value validation
            bool ok = true;
            RecipeItem item = (RecipeItem)itemIndex;
            switch (item)
            {
                case RecipeItem.TactTime:
                    {
                        int tacttime = 0;
                        if (int.TryParse(value, out tacttime))
                        {
                            ok &= (tacttime >= 38 && tacttime <= 70); // 11.06.01 minhan
                        }
                        else ok = false;
                    }
                    break;
                case RecipeItem.CvSpeed:
                    {
                        int speed = 0;
                        if (int.TryParse(value, out speed))
                        {
                            ok &= (speed >= 1128 && speed <= 11280);
                        }
                        else ok = false;
                    }
                    break;
                case RecipeItem.HpmjPressure:
                    {
                        int Pressure = 0;
                        if (int.TryParse(value, out Pressure))
                        {
                            ok &= (Pressure >= 80 && Pressure <= 130); // 11.04.20 minhan
                        }
                        else ok = false;
                    }
                    break;
                //case RecipeItem.ApVoltage:
                //    {
                //        double voltage = 0;
                //        if (double.TryParse(value, out voltage))
                //        {
                //            ok &= (voltage >= 7 && voltage <= 13) ; // 11.04.20 minhan
                //        }
                //        else
                //        {
                //            ok = false;
                //        }
                //    }
                //    break;
                //case RecipeItem.ApN2Flow: // 10.12.05 taegoo
                //    {
                //        //double Flow = Convert.ToDouble(value);//10.12.05 taegoo

                //        double Flow = 0;
                //        if (double.TryParse(value, out Flow))
                //        {
                //            ok &= (Flow >= 700 && Flow <= 1500) ;
                //        }
                //        else
                //        {
                //            ok = false;
                //        }
                //    }
                //    break;
                //case RecipeItem.ApCDAFlow: // 10.12.05 taegoo
                //    {
                //        //double Flow = Convert.ToDouble(value);//10.12.05 taegoo

                //        double Flow = 0;
                //        if (double.TryParse(value, out Flow))
                //        {
                //            ok &= (Flow >= 0 && Flow <= 10) ; // 11.03.24 minhan
                //        }
                //        else
                //        {
                //            ok = false;
                //        }
                //    }
                //    break;
                case RecipeItem.RB1UpSpeed:
                    {
                        double speed = 0;
                        if (double.TryParse(value, out speed))
                        {
                            ok &= (speed >= 100 && speed <= 600);
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    break;
                case RecipeItem.RB1LoSpeed:
                    {
                        double speed = 0;
                        if (double.TryParse(value, out speed))
                        {
                            ok &= (speed >= 100 && speed <= 600);
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    break;
                case RecipeItem.RB2UpSpeed:
                    {
                        double speed = 0;
                        if (double.TryParse(value, out speed))
                        {
                            ok &= (speed >= 100 && speed <= 600);
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    break;
                case RecipeItem.RB2LoSpeed:
                    {
                        double speed = 0;
                        if (double.TryParse(value, out speed))
                        {
                            ok &= (speed >= 100 && speed <= 600);
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    break;
                case RecipeItem.RB1UpGap:
                    {
                        double gap = 0;
                        if (double.TryParse(value, out gap))
                        {
                            ok &= (gap >= 0 && gap <= 3);
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    break;
                case RecipeItem.RB1LoGap:
                    {
                        double gap = 0;
                        if (double.TryParse(value, out gap))
                        {
                            ok &= (gap >= 0 && gap <= 3);
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    break;
                case RecipeItem.RB2UpGap:
                    {
                        double gap = 0;
                        if (double.TryParse(value, out gap))
                        {
                            ok &= (gap >= 0 && gap <= 3);
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    break;
                case RecipeItem.RB2LoGap:
                    {
                        double gap = 0;
                        if (double.TryParse(value, out gap))
                        {
                            ok &= (gap >= 0 && gap <= 3);
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    break;
            }

            return ok;
        }

        public static bool CheckRecipeItem(TagRecipe item) // 10.12.25 minhan recipe 아이템 설정값 검색추가.
        {
            bool ok = true;

            ok = ((item.TactTime >= 38) && (item.TactTime <= 70)); // 11.06.01 minhan
            ok &= (item.CvSpeed >= 1128 && item.CvSpeed <= 11280);

            //if (item.ApUse)
            //{
            //    ok &= (item.ApVoltage >= 7 && item.ApVoltage <= 13) ; // 11.04.20 minhan
            //    ok &= (item.ApN2Flow >= 700 && item.ApN2Flow <= 1500) ;
            //    ok &= (item.ApCDAFlow >= 0 && item.ApCDAFlow <= 10) ; // 11.03.24 minhan
            //}

            if (item.HpmjUse) // 11.03.02 minhan
            {
                ok &= (item.HpmjPressure >= 80 && item.HpmjPressure <= 130); // 11.04.20 minhan
            }

            if (item.RB1UpUse)
            {
                ok &= (item.RB1UpSpeed >= 100 && item.RB1UpSpeed <= 600);
                ok &= (item.RB1UpGap >= 0 && item.RB1UpGap <= 3);
            }

            if (item.RB1LoUse)
            {
                ok &= (item.RB1LoSpeed >= 100 && item.RB1LoSpeed <= 600);
                ok &= (item.RB1LoGap >= 0 && item.RB1LoGap <= 3);
            }

            if (item.RB2UpUse)
            {
                ok &= (item.RB2UpSpeed >= 100 && item.RB2UpSpeed <= 600);
                ok &= (item.RB2UpGap >= 0 && item.RB2UpGap <= 3);
            }

            if (item.RB2LoUse)
            {
                ok &= (item.RB2LoSpeed >= 100 && item.RB2LoSpeed <= 600);
                ok &= (item.RB2LoGap >= 0 && item.RB2LoGap <= 3);
            }

            return ok;
        }
    }
}
