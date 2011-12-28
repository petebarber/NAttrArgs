//
// NAttrArgs
//
// Copyright (c) 2012 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System;
using System.Collections.Generic;

namespace NAttrArgs
{
	public class ArgIterator : IEnumerator<string>
	{
		private const int INDEX_BEFORE_FIRST_ITEM = -1;
		private readonly string[] _args;
		private int _currentIndex = INDEX_BEFORE_FIRST_ITEM;
		private readonly int INDEX_AFTER_LAST_ITEM;
		private readonly int INDEX_OF_LAST_ITEM;

		public ArgIterator(string[] args)
		{
			ThrowIfArgsAreInvalid(args);
			
			_args = args;

			INDEX_OF_LAST_ITEM = _args.Length - 1;
			INDEX_AFTER_LAST_ITEM = _args.Length;
		}

		private void ThrowIfArgsAreInvalid(string[] args)
		{
			if (args == null)
				throw new ArgumentException();
			else if (args.Length >= int.MaxValue)
				throw new ArgumentException("array too long");
		}

		public string Current
		{
			get 
			{
				if (_currentIndex <= INDEX_BEFORE_FIRST_ITEM || _currentIndex >= INDEX_AFTER_LAST_ITEM)
					return null;
				else
					return _args[_currentIndex];
			}
		}

		public void Dispose()
		{
		}

		object System.Collections.IEnumerator.Current
		{
			get { return Current; }
		}

		public bool MoveNext()
		{
			if (_currentIndex < INDEX_OF_LAST_ITEM)
			{
				++_currentIndex;
				return true;
			}
			else
			{
				_currentIndex = INDEX_AFTER_LAST_ITEM;
				return false;
			}
		}

		public void Reset()
		{
			_currentIndex = INDEX_BEFORE_FIRST_ITEM;
		}

		public bool MoveBack()
		{
			if (_currentIndex > INDEX_BEFORE_FIRST_ITEM)
			{
				--_currentIndex;
				return true;
			}
			else
				return false;
		}
	}
}
