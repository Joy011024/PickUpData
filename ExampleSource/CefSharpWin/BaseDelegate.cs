using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace CefSharpWin
{
    [Description("回调事件")]
    public delegate void CallTodo(object obj);
    [Description("获取到cookie之后执行的事件")]
    public delegate void  GetCookieTodo(object obj);
}
