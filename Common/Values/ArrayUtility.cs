using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.Common.Values;

/// <summary>配列ユーティリティ</summary>
public static class ArrayUtility
{
	/// <summary>複数の配列から各要素を組み立る</summary>
	/// <typeparam name="T1">一つ目の配列の要素の型</typeparam>
	/// <typeparam name="T2">二つ目の配列の要素の型</typeparam>
	/// <typeparam name="TResult">組み立てた結果</typeparam>
	/// <param name="assmbleMethod">組み立てメソッド</param>
	/// <param name="array1">一つ目の配列</param>
	/// <param name="array2">二つ目の配列</param>
	/// <returns>結果の配列</returns>
	/// <exception cref="ArgumentException">要素数が異なる</exception>
	public static IEnumerable<TResult> Assemble<T1, T2, TResult>(
		Func<T1, T2, TResult> assmbleMethod,
		IEnumerable<T1> array1, IEnumerable<T2> array2)
	{
		if (!EqualsAll(array1.Count(), array2.Count())) { throw new ArgumentException("要素数が異なる"); }
		return array1.Select((elem, i) => assmbleMethod.Invoke(elem, array2.ElementAt(i)));
	}

	/// <summary>複数の配列から各要素を組み立る</summary>
	/// <typeparam name="T1">一つ目の配列の要素の型</typeparam>
	/// <typeparam name="T2">二つ目の配列の要素の型</typeparam>
	/// <typeparam name="T3">三つ目の配列の要素の型</typeparam>
	/// <typeparam name="TResult">組み立てた結果</typeparam>
	/// <param name="assmbleMethod">組み立てメソッド</param>
	/// <param name="array1">一つ目の配列</param>
	/// <param name="array2">二つ目の配列</param>
	/// <param name="array3">三つ目の配列</param>
	/// <returns>結果の配列</returns>
	/// <exception cref="ArgumentException">要素数が異なる</exception>
	public static IEnumerable<TResult> Assemble<T1, T2, T3, TResult>(
		Func<T1, T2, T3, TResult> assmbleMethod,
		IEnumerable<T1> array1, IEnumerable<T2> array2, IEnumerable<T3> array3)
	{
		if (!EqualsAll(array1.Count(), array2.Count(), array3.Count())) { throw new ArgumentException("要素数が異なる"); }
		return array1.Select((elem, i) => assmbleMethod.Invoke(elem, array2.ElementAt(i), array3.ElementAt(i)));
	}

	/// <summary>複数の配列から各要素を組み立る</summary>
	/// <typeparam name="T1">一つ目の配列の要素の型</typeparam>
	/// <typeparam name="T2">二つ目の配列の要素の型</typeparam>
	/// <typeparam name="T3">三つ目の配列の要素の型</typeparam>
	/// <typeparam name="T4">四つ目の配列の要素の型</typeparam>
	/// <typeparam name="TResult">組み立てた結果</typeparam>
	/// <param name="assmbleMethod">組み立てメソッド</param>
	/// <param name="array1">一つ目の配列</param>
	/// <param name="array2">二つ目の配列</param>
	/// <param name="array3">三つ目の配列</param>
	/// <param name="array4">四つ目の配列</param>
	/// <returns>結果の配列</returns>
	/// <exception cref="ArgumentException">要素数が異なる</exception>
	public static IEnumerable<TResult> Assemble<T1, T2, T3, T4, TResult>(
		Func<T1, T2, T3, T4, TResult> assmbleMethod,
		IEnumerable<T1> array1, IEnumerable<T2> array2, IEnumerable<T3> array3, IEnumerable<T4> array4)
	{
		if (!EqualsAll(array1.Count(), array2.Count(), array3.Count(), array4.Count())) { throw new ArgumentException("要素数が異なる"); }
		return array1.Select((elem, i) => assmbleMethod.Invoke(elem, array2.ElementAt(i), array3.ElementAt(i), array4.ElementAt(i)));
	}

	/// <summary>全て同値かを返す</summary>
	/// <typeparam name="T">要素の型</typeparam>
	/// <param name="array">対象の配列</param>
	/// <returns>全て同値か</returns>
	public static bool EqualsAll<T>(params T[] array)
	{
		var count = array.Length;
		if (count <= 0)
		{
			return false;
		}

		var first = array[0];
		foreach (var elem in array)
		{
			if (!object.Equals(first, elem))
			{
				return false;
			}
		}

		return true;
	}
}
