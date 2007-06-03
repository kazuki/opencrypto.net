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

#if TEST

using System;
using System.Threading;
using NUnit.Framework;

namespace openCrypto.Tests
{
	[TestFixture]
	public class ConfluentWaitHandleTest
	{
		[Test]
		public void TestA ()
		{
			ConfluentWaitHandle handle = new ConfluentWaitHandle ();
			Thread[] list = new Thread[] {
				new Thread (sleepThread), new Thread (sleepThread)
			};

			for (int i = 0; i < list.Length; i ++) {
				handle.StartThread ();
				list[i].Start (handle);
			}

			DateTime dt = DateTime.Now;
			handle.WaitOne ();
			double t = DateTime.Now.Subtract (dt).TotalSeconds;

			// こんな適当なチェックでいいのかな…
			Assert.IsTrue (t >= 0.8 && t <= 1.2, t.ToString ());
		}

		void sleepThread (Object o)
		{
			ConfluentWaitHandle handle = (ConfluentWaitHandle)o;
			Thread.Sleep (1000);
			handle.EndThread ();
		}
	}
}
#endif
