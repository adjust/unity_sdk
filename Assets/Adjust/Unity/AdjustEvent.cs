using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	public class AdjustEvent
	{
		internal double? revenue;

		internal string currency;
		internal string eventToken;

		internal List<string> partnerList;
		internal List<string> callbackList;

		public AdjustEvent(string eventToken)
		{
			this.eventToken = eventToken;
		}

		public void setRevenue(double amount, string currency)
		{
			this.revenue = amount;
			this.currency = currency;
		}

		public void addCallbackParameter(string key, string value)
		{
			if (callbackList == null) {
				callbackList = new List<string>();
			}

			callbackList.Add(key);
			callbackList.Add(value);
		}

		public void addPartnerParameter(string key, string value)
		{
			if (partnerList == null) {
				partnerList = new List<string>();
			}

			partnerList.Add(key);
			partnerList.Add(value);
		}
	}
}
