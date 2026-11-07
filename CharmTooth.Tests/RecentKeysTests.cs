using CharmTooth.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CharmTooth.Tests;

[TestClass]
public class RecentKeysTests
{
	[TestMethod]
	public void TryAdd_BlocksTheSameKeyUntilItExpires()
	{
		RecentKeys keys = new RecentKeys();
		Assert.IsTrue(keys.TryAdd(7, 10f, 3f));
		Assert.IsFalse(keys.TryAdd(7, 11f, 3f));
		Assert.IsTrue(keys.Contains(7, 12.9f));
		Assert.IsFalse(keys.Contains(7, 13f));
		Assert.IsTrue(keys.TryAdd(7, 13f, 3f));
	}

	[TestMethod]
	public void Prune_AndRemoveAndClear_ForgetKeys()
	{
		RecentKeys keys = new RecentKeys();
		keys.TryAdd(1, 0f, 1f);
		keys.TryAdd(2, 0f, 5f);
		keys.TryAdd(3, 0f, 5f);
		keys.Prune(2f);
		Assert.AreEqual(2, keys.Count);
		keys.Remove(2);
		Assert.IsFalse(keys.Contains(2, 2f));
		keys.Clear();
		Assert.AreEqual(0, keys.Count);
	}
}
