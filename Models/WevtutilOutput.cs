using System;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace WLP.Models
{
	[XmlRoot(ElementName = "Provider", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
	public class Provider
	{
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "Guid")]
		public string Guid { get; set; }
	}

	[XmlRoot(ElementName = "TimeCreated", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
	public class TimeCreated
	{
		[XmlAttribute(AttributeName = "SystemTime")]
		public string SystemTime { get; set; }
	}

	[XmlRoot(ElementName = "Correlation", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
	public class Correlation
	{
		[XmlAttribute(AttributeName = "ActivityID")]
		public string ActivityID { get; set; }
	}

	[XmlRoot(ElementName = "Execution", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
	public class Execution
	{
		[XmlAttribute(AttributeName = "ProcessID")]
		public string ProcessID { get; set; }
		[XmlAttribute(AttributeName = "ThreadID")]
		public string ThreadID { get; set; }
	}

	[XmlRoot(ElementName = "Security", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
	public class Security
	{
		[XmlAttribute(AttributeName = "UserID")]
		public string UserID { get; set; }
	}

	[XmlRoot(ElementName = "System", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
	public class System
	{
		[XmlElement(ElementName = "Provider", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public Provider Provider { get; set; }
		[XmlElement(ElementName = "EventID", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public string EventID { get; set; }
		[XmlElement(ElementName = "Version", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public string Version { get; set; }
		[XmlElement(ElementName = "Level", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public string Level { get; set; }
		[XmlElement(ElementName = "Task", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public string Task { get; set; }
		[XmlElement(ElementName = "Opcode", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public string Opcode { get; set; }
		[XmlElement(ElementName = "Keywords", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public string Keywords { get; set; }
		[XmlElement(ElementName = "TimeCreated", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public TimeCreated TimeCreated { get; set; }
		[XmlElement(ElementName = "EventRecordID", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public string EventRecordID { get; set; }
		[XmlElement(ElementName = "Correlation", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public Correlation Correlation { get; set; }
		[XmlElement(ElementName = "Execution", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public Execution Execution { get; set; }
		[XmlElement(ElementName = "Channel", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public string Channel { get; set; }
		[XmlElement(ElementName = "Computer", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public string Computer { get; set; }
		[XmlElement(ElementName = "Security", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public Security Security { get; set; }
	}

	[XmlRoot(ElementName = "Data", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
	public class Data
	{
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "EventData", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
	public class EventData
	{
		[XmlElement(ElementName = "Data", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public Data Data { get; set; }
	}

	[XmlRoot(ElementName = "Event", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
	public class Event
	{
		[XmlElement(ElementName = "System", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public System System { get; set; }
		[XmlElement(ElementName = "EventData", Namespace = "http://schemas.microsoft.com/win/2004/08/events/event")]
		public EventData EventData { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}
}
