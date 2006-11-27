// 
// Copyright (c) 2006, Kazuki Oikawa
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Threading;

namespace openCrypto
{
	/// <summary>
	/// <ja>複数のスレッドの終了を待つクラス</ja>
	/// </summary>
	class ConfluentWaitHandle
	{
		ManualResetEvent _handle;
		int _counter;

		public ConfluentWaitHandle ()
		{
			_handle = new ManualResetEvent (true);
			_counter = 0;
		}

		/// <summary>
		/// <ja>終了を待機するスレッドを追加するメソッド</ja>
		/// </summary>
		public void StartThread ()
		{
			_handle.Reset ();
			Interlocked.Increment (ref _counter);
		}

		/// <summary>
		/// <ja>待機しているスレッドが終了したことを通知するメソッド</ja>
		/// </summary>
		public void EndThread ()
		{
			if (Interlocked.Decrement (ref _counter) == 0)
				_handle.Set ();
		}

		/// <summary>
		/// <ja>登録されているスレッドがすべて終了するまでブロックするメソッド</ja>
		/// </summary>
		/// <returns>
		/// <ja>trueなら正常にすべてのスレッドが終了、falseならスレッドの終了を待たずにこのインスタンスが閉じられた</ja>
		/// </returns>
		public bool WaitOne ()
		{
			return _handle.WaitOne ();
		}

		/// <summary>
		/// <ja>このインスタンスが保持しているリソースを開放するメソッド</ja>
		/// </summary>
		public void Close () {
			_handle.Close ();
		}
	}
}
