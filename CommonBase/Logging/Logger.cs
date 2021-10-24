using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CreditLineDemoAPI.CommonBase.Logging
{
    public class Logger
    {
        private static log4net.ILog Log { get; set; }

        static Logger()
        {
            Log = log4net.LogManager.GetLogger(typeof(Logger));
        }

        public static void Info()
        {
            Log.Info(ParseMessage());
        }

        public static void Info(object msg)
        {
            Log.Info(ParseMessage(msg));
        }

        public static void Info(object msg, Exception ex)
        {
            Log.Info(ParseMessage(msg), ex);
        }

        public static void Info(Exception ex)
        {
            Log.Info(ParseMessage(ex.Message), ex);
        }

        public static void Warn(object msg)
        {
            Log.Warn(ParseMessage(msg));
        }

        public static void Warn(object msg, Exception ex)
        {
            Log.Warn(ParseMessage(msg), ex);
        }

        public static void Warn(Exception ex)
        {
            Log.Warn(ParseMessage(ex.Message), ex);
        }

        public static void Error(object msg)
        {
            Log.Error(ParseMessage(msg));
        }

        public static void Error(object msg, Exception ex)
        {
            Log.Error(ParseMessage(msg), ex);
        }

        public static void Error(Exception ex)
        {
            Log.Error(ParseMessage(ex.Message), ex);
        }

        public static void Fatal(object msg)
        {
            Log.Fatal(ParseMessage(msg));
        }

        public static void Fatal(object msg, Exception ex)
        {
            Log.Fatal(ParseMessage(msg), ex);
        }

        public static void Fatal(Exception ex)
        {
            Log.Fatal(ParseMessage(ex.Message), ex);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ParseMessage()
        {
            string str = string.Empty;
            try
            {
                StackTrace stackTrace = new StackTrace();
                StackFrame frame = stackTrace.GetFrame(2);
                MethodBase method = frame.GetMethod();

                if (method.DeclaringType != null)
                    str = $"{method.DeclaringType.FullName}.{method.Name}";
            }
            catch (Exception ex)
            {
                Trace.TraceError("Logger Exception:{0}", (object)ex);
            }
            return str;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ParseMessage(object message)
        {
            string str = string.Empty;
            try
            {
                StackTrace stackTrace = new StackTrace();
                StackFrame frame = stackTrace.GetFrame(2);
                MethodBase method = frame.GetMethod();

                if (method.DeclaringType != null)
                    str = $"{method.DeclaringType.FullName}.{method.Name}:{message}";
            }
            catch (Exception ex)
            {
                Trace.TraceError("Logger Exception:{0}", (object)ex);
            }
            return str;
        }

    }
}