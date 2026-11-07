using CharmTooth.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CharmTooth.Tests;

[TestClass]
public class HealMathTests
{
	[DataTestMethod]
	[DataRow(100f, 1f, 1)]
	[DataRow(1000f, 1f, 10)]
	[DataRow(250f, 1f, 3)]
	[DataRow(240f, 1f, 2)]
	[DataRow(350f, 1f, 4)]
	[DataRow(1000f, 0.5f, 5)]
	[DataRow(400f, 25f, 100)]
	public void HealAmount_RoundsToNearestPoint(float value, float rate, int expected)
	{
		Assert.AreEqual(expected, HealMath.HealAmount(value, rate));
	}

	[DataTestMethod]
	[DataRow(1f, 1f)]
	[DataRow(10f, 1f)]
	[DataRow(49f, 1f)]
	[DataRow(100f, 0.1f)]
	public void HealAmount_IsAtLeastOneWhenValueAndRateArePositive(float value, float rate)
	{
		Assert.IsTrue(HealMath.HealAmount(value, rate) >= 1);
	}

	[DataTestMethod]
	[DataRow(0f, 1f)]
	[DataRow(-50f, 1f)]
	[DataRow(500f, 0f)]
	[DataRow(500f, -1f)]
	[DataRow(float.NaN, 1f)]
	public void HealAmount_IsZeroWithNothingToHeal(float value, float rate)
	{
		Assert.AreEqual(0, HealMath.HealAmount(value, rate));
	}

	[TestMethod]
	public void HealAmount_HugeValuesDoNotOverflow()
	{
		Assert.IsTrue(HealMath.HealAmount(float.MaxValue, 25f) > 0);
	}

	[DataTestMethod]
	[DataRow(50, 100, false, true)]
	[DataRow(100, 100, false, false)]
	[DataRow(100, 100, true, true)]
	public void MayEat_RespectsFullHealthSetting(int health, int max, bool atFull, bool expected)
	{
		Assert.AreEqual(expected, HealMath.MayEat(health, max, atFull));
	}
}
