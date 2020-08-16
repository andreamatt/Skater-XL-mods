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
		// first has highest priority
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

		public void AddFirst(string name, bool enabled) {
			names_enables.Insert(0, (name, enabled));
		}

		public void Add(string name, bool enabled) {
			names_enables.Add((name, enabled));
		}

		public void Remove(string name) {
			names_enables.RemoveAll(n_e => n_e.Item1 == name);
		}

		public void Upgrade(string name) {
			var i = names_enables.FindIndex(n_e => n_e.Item1 == name);
			var tmp_name = names_enables[i];
			names_enables[i] = names_enables[i - 1];
			names_enables[i - 1] = tmp_name;
		}

		public void Downgrade(string name) {
			var i = names_enables.FindIndex(n_e => n_e.Item1 == name);
			var tmp_name = names_enables[i];
			names_enables[i] = names_enables[i + 1];
			names_enables[i + 1] = tmp_name;
		}
	}
}
