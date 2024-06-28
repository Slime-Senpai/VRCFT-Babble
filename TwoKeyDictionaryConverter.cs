using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VRCFaceTracking.Babble.Collections;

namespace VRCFaceTracking.Babble;

public class TwoKeyDictionaryConverter<TKey1, TKey2, TValue> : JsonConverter<TwoKeyDictionary<TKey1, TKey2, TValue>>
{
	public override bool CanRead => true;

	public override bool CanWrite => true;

	public override void WriteJson(JsonWriter writer, TwoKeyDictionary<TKey1, TKey2, TValue> tkd,
		JsonSerializer serializer)
	{
		var list = new List<JObject>();
		for (var i = 0; i < tkd.Count; i++)
		{
			var tuple = tkd.ElementAt(i);
			var item = new JObject(new JProperty("unifiedExpression", tuple.Item1.ToString()),
				new JProperty("oscAddress", tuple.Item2), new JProperty("weight", tuple.Item3));
			list.Add(item);
		}

		serializer.Serialize(writer, list);
	}

	public override TwoKeyDictionary<TKey1, TKey2, TValue> ReadJson(JsonReader reader, Type objectType,
		TwoKeyDictionary<TKey1, TKey2, TValue> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		var list = serializer.Deserialize<List<JObject>>(reader);
		if (list == null) return null;
		var twoKeyDictionary = new TwoKeyDictionary<TKey1, TKey2, TValue>();
		foreach (var item in list)
		{
			var key = (TKey1)Enum.Parse(typeof(TKey1), item["unifiedExpression"].Value<string>());
			var key2 = item["oscAddress"].Value<TKey2>();
			var value = item["weight"].Value<TValue>();
			twoKeyDictionary.Add(key, key2, value);
		}

		return twoKeyDictionary;
	}
}