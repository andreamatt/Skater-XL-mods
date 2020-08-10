using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLGraphics.Presets
{
	public class PresetSelection
	{
		public List<(string, bool)> names_enables = new List<(string, bool)>();

		[JsonIgnore]
		public ReadOnlyCollection<string> Names => names_enables.Select(n_e => n_e.Item1).ToList().AsReadOnly();
		[JsonIgnore]
		public ReadOnlyCollection<bool> Enables => names_enables.Select(n_e => n_e.Item2).ToList().AsReadOnly();

		public bool IsEnabled(string name) {
			if (!Names.Contains(name)) {
				throw new Exception("No such preset found: " + name);
			}
			return names_enables.First(n_e => n_e.Item1 == name).Item2;
		}

		public void SetEnabled(string name, bool enabled) {
			if (!Names.Contains(name)) {
				throw new Exception("No such preset found: " + name);
			}
			var i = names_enables.FindIndex(n_e => n_e.Item1 == name);
			names_enables[i] = (name, enabled);
		}

		public void Add(string name, bool enabled) {
			names_enables.Add((name, enabled));
		}

		public void Remove(string name) {
			names_enables.RemoveAll(n_e => n_e.Item1 == name);
		}

		public int Count => names_enables.Count;

		public void Left(int i) {
			var tmp_name = names_enables[i];
			names_enables[i] = names_enables[i - 1];
			names_enables[i - 1] = tmp_name;
		}

		public void Right(int i) {
			var tmp_name = names_enables[i];
			names_enables[i] = names_enables[i + 1];
			names_enables[i + 1] = tmp_name;
		}
	}
}
