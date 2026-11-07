using System.Collections.Generic;
using CharmTooth.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CharmTooth.Tests;

[TestClass]
public class FoodListTests
{
	[TestMethod]
	public void Parse_SplitsTrimsAndDropsEmptyEntries()
	{
		HashSet<string> set = FoodList.Parse(" A b ,, C(Clone) ,  ");
		Assert.AreEqual(2, set.Count);
		Assert.IsTrue(set.Contains("A b"));
		Assert.IsTrue(set.Contains("C"));
	}

	[DataTestMethod]
	[DataRow(null)]
	[DataRow("")]
	[DataRow("  , ,")]
	public void Parse_EmptyInputGivesEmptySet(string? text)
	{
		Assert.AreEqual(0, FoodList.Parse(text).Count);
	}

	[DataTestMethod]
	[DataRow("Valuable Museum Toast(Clone)", true)]
	[DataRow("valuable museum toast", true)]
	[DataRow("  Valuable Museum Toast (Clone)(Clone) ", true)]
	[DataRow("Valuable Museum Tooth", false)]
	[DataRow("", false)]
	[DataRow(null, false)]
	public void IsFood_MatchesCaseInsensitivelyWithoutClone(string? name, bool expected)
	{
		HashSet<string> foods = FoodList.Parse(FoodList.DefaultNames);
		Assert.AreEqual(expected, FoodList.IsFood(foods, name));
	}

	[TestMethod]
	public void DefaultNames_ContainTheEightFoodPrefabs()
	{
		Assert.AreEqual(8, FoodList.Parse(FoodList.DefaultNames).Count);
	}
}
