using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json;

namespace RK800.Save
{

    public abstract class SaveFile<T> : ISaveFile
    {
        public T Data;
        public SaveFile(FileInfo file) : base(file) { }
    }

    public class UlongSaveFile : SaveFile<List<ulong>>
    {
        public override void Read()
        {
            Data = new List<ulong>();
            if (JsonConvert.DeserializeObject(System.IO.File.ReadAllText(File.FullName), typeof(List<ulong>)) is List<ulong> FileData) Data = FileData;
        }

        public override void Write() => System.IO.File.WriteAllText(File.FullName, JsonConvert.SerializeObject(Data));

        public UlongSaveFile(FileInfo file) : base(file) { }
    }

    public class UlongStringSaveFile : SaveFile<List<UlongString>>
    {
        public override void Read()
        {
            Data = new List<UlongString>();
            if (JsonConvert.DeserializeObject(System.IO.File.ReadAllText(File.FullName), typeof(List<UlongString>)) is List<UlongString> FileData) Data = FileData;
        }

        public override void Write() => System.IO.File.WriteAllText(File.FullName, JsonConvert.SerializeObject(Data));

        public UlongStringSaveFile(FileInfo file) : base(file) { }
    }

    public class FilterSaveFile : SaveFile<Dictionary<ulong, FilterData>>
    {
        public override void Read()
        {
            Data = new Dictionary<ulong, FilterData>();
            if (JsonConvert.DeserializeObject(System.IO.File.ReadAllText(File.FullName), typeof(Dictionary<ulong, FilterData>)) is Dictionary<ulong, FilterData> FileData) Data = FileData;
        }

        public override void Write() => System.IO.File.WriteAllText(File.FullName, JsonConvert.SerializeObject(Data));

        public FilterSaveFile(FileInfo file) : base(file) { }
    }

    public class TrackerSaveFile : SaveFile<Dictionary<ulong, TrackerData>>
    {
        public override void Read()
        {
            Data = new Dictionary<ulong, TrackerData>();
            Dictionary<ulong, TrackerData> FileData = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(File.FullName), typeof(Dictionary<ulong, TrackerData>)) as Dictionary<ulong, TrackerData>;
            if (FileData != null) Data = FileData;
            //manually update time
            foreach (TrackerData data in Data.Values)
                data.dt = DateTime.Now;
        }

        public override void Write() => System.IO.File.WriteAllText(File.FullName, JsonConvert.SerializeObject(Data));

        public TrackerSaveFile(FileInfo file) : base(file) { }
    }

    public class UlongString
    {
        public ulong ul;
        public string str;


        public UlongString(ulong u64, string s)
        {
            ul = u64;
            str = s;
        }
    }

    public class FilterData
    {
        public List<string> Words;

        public bool IsEnabled = true;

        public FilterData(List<string> list) => Words = list;
    }

    public class TrackerData
    {
        public DateTime dt;
        public TimeSpan ts;
        public string msg;
        public bool IsTrackerEnabled;
        public bool IsAlertEnabled;

        public TrackerData(DateTime date, TimeSpan time, string s = null, bool tracker = false, bool alert = false)
        {
            dt = date;
            ts = time;
            msg = s;
            IsTrackerEnabled = tracker;
            IsAlertEnabled = alert;
        }
    }
}