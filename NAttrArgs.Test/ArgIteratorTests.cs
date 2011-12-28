//
// NAttrArgs
//
// Copyright (c) 2012 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace NAttrArgs.Test
{
	class ArgIteratorTests
	{
		private ArgIterator m_emptyIterator;
		private ArgIterator m_singleItemIterator;
		private ArgIterator m_fiveItemIterator;

		ArgIterator MakeItemIterator(int numOfItems)
		{
			return new ArgIterator(Enumerable.Range(0, numOfItems).Select(n => n.ToString()).ToArray());
		}

		[SetUp]
		public void InitializeSamples()
		{
			m_emptyIterator = new ArgIterator(new string[] { });
			m_singleItemIterator = new ArgIterator(new string[] { "one" });
			m_fiveItemIterator = new ArgIterator(new string[] { "1", "2", "3", "4", "5" });
		}

		[Test]
		public void ArgIteratorConstructor_WithNull_ThrowsArgumentException()
		{
			Assert.That(()=> { new ArgIterator(null); }, Throws.TypeOf<ArgumentException>());
		}

		[Test]
		public void ArgIteratorConstructor_CanInstaniate()
		{
			Assert.That(m_emptyIterator, Is.Not.Null);
		}

		[Test]
		public void MoveNext_WhenEmpty_ReturnsFalse()
		{
			Assert.That(m_emptyIterator.MoveNext(), Is.False);
		}

		[Test]
		public void MoveBack_WhenEmpty_ReturnsFalse()
		{
			Assert.That(m_emptyIterator.MoveBack(), Is.False);
		}

		[Test]
		public void Current_WhenEmptyCurrent_Returns_null()
		{
			Assert.That(m_emptyIterator.Current, Is.Null);
		}

		[Test]
		public void Current_WhenEmptyIEnumeratorCurrent_Returns_null()
		{
			Assert.That(((IEnumerator)m_emptyIterator).Current, Is.Null);
		}

		[Test] 
		public void Current_BeforeMoveNext_CurrentReturnsNull()
		{
			Assert.That(m_singleItemIterator.Current, Is.Null);
		}

		[Test]
		public void MoveNext_FirstCallAdvancesToFirstItem_CurrentIsOne()
		{
			Assert.That(m_singleItemIterator.MoveNext(), Is.True);
			Assert.That(m_singleItemIterator.Current, Is.EqualTo("one"));
		}

		[Test]
		public void MoveBack_AtferMoveNextWhenThereIsASingleItem_CurrentReturnsNull()
		{
			Assert.That(m_singleItemIterator.MoveNext(), Is.True);
			Assert.That(m_singleItemIterator.Current, Is.EqualTo("one"));
			Assert.That(m_singleItemIterator.MoveBack(), Is.True);
			Assert.That(m_singleItemIterator.Current, Is.Null);
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(5)]
		[TestCase(10)]
		public void MoveNext_AfterNCalls_ReturnsFalse(int numOfItems)
		{
			var itemIterator = MakeItemIterator(numOfItems);

			for (int count = 0; count < numOfItems; ++count)
				Assert.That(itemIterator.MoveNext(), Is.True);

			Assert.That(itemIterator.MoveNext(), Is.False);			
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(5)]
		[TestCase(10)]
		public void MoveNext_Current_HasExpectedValue(int numOfItems)
		{
			var itemIterator = MakeItemIterator(numOfItems);

			for (int count = 0; count < numOfItems; ++count)
			{
				Assert.That(itemIterator.MoveNext(), Is.True);
				Assert.That(itemIterator.Current, Is.EqualTo(count.ToString()));
			}

			Assert.That(itemIterator.MoveNext(), Is.False);
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(5)]
		[TestCase(10)]
		public void MoveBack_Current_HasExpectedValue(int numOfItems)
		{
			var itemIterator = MakeItemIterator(numOfItems);

			// Move to end first
			for (int count = 0; count < numOfItems; ++count)
				Assert.That(itemIterator.MoveNext(), Is.True);

			Assert.That(itemIterator.MoveNext(), Is.False);

			for (int count = numOfItems - 1; count >= 0; --count)
			{
				Assert.That(itemIterator.MoveBack(), Is.True);
				Assert.That(itemIterator.Current, Is.EqualTo(count.ToString()));
			}

			Assert.That(itemIterator.MoveBack(), Is.True);
			Assert.That(itemIterator.Current, Is.Null);
			Assert.That(itemIterator.MoveBack(), Is.False);
		}
	}
}
