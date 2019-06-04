using System;
using System.Drawing;
using System.Runtime.InteropServices;
using MasterChief.DotNet4.WindowsAPI.Enum;

namespace MasterChief.DotNet4.WindowsAPI.Model
{
    // ReSharper disable once InconsistentNaming
    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessId;
        public int dwThreadId;
    }

    // ReSharper disable once InconsistentNaming
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct STARTUPINFO
    {
        public int cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwYSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    internal struct INPUT
    {
        #region Fields

        public MOUSEKEYBDHARDWAREINPUT Data;
        public uint Type;

        #endregion Fields
    }

    internal struct MOUSEINPUT
    {
        #region Fields

        public IntPtr ExtraInfo;
        public uint Flags;
        public uint MouseData;
        public uint Time;
        public int X;
        public int Y;

        #endregion Fields
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct MOUSEKEYBDHARDWAREINPUT
    {
        [FieldOffset(0)] public MOUSEINPUT Mouse;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Rect
    {
        public Rect(Rect rectangle)
            : this(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
        {
        }

        public Rect(int left, int top, int right, int bottom)
        {
            X = left;
            Y = top;
            Right = right;
            Bottom = bottom;
        }


        public int Bottom { get; set; }

        public int Height
        {
            get => Bottom - Y;
            set => Bottom = value + Y;
        }

        public int Left
        {
            get => X;
            set => X = value;
        }

        public Point Location
        {
            get => new Point(Left, Top);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public int Right { get; set; }

        public Size Size
        {
            get => new Size(Width, Height);
            set
            {
                Right = value.Width + X;
                Bottom = value.Height + Y;
            }
        }

        public int Top
        {
            get => Y;
            set => Y = value;
        }

        public int Width
        {
            get => Right - X;
            set => Right = value + X;
        }

        public int X { get; set; }

        public int Y { get; set; }


        public static implicit operator Rectangle(Rect rectangle)
        {
            return new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
        }

        public static implicit operator Rect(Rectangle rectangle)
        {
            return new Rect(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }

        public static bool operator !=(Rect rectangle1, Rect rectangle2)
        {
            return !rectangle1.Equals(rectangle2);
        }

        public static bool operator ==(Rect rectangle1, Rect rectangle2)
        {
            return rectangle1.Equals(rectangle2);
        }

        public bool Equals(Rect rectangle)
        {
            return rectangle.Left == X && rectangle.Top == Y && rectangle.Right == Right && rectangle.Bottom == Bottom;
        }

        public override bool Equals(object Object)
        {
            if (Object is Rect) return Equals((Rect) Object);

            if (Object is Rectangle) return Equals(new Rect((Rectangle) Object));

            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "{Left: " + X + "; " + "Top: " + Y + "; Right: " + Right + "; Bottom: " + Bottom + "}";
        }
    }

    // ReSharper disable once InconsistentNaming
    [StructLayout(LayoutKind.Sequential)]
    internal struct WTS_SESSION_INFO
    {
        public readonly int SessionID;
        [MarshalAs(UnmanagedType.LPStr)] public readonly string pWinStationName;
        public readonly WTS_CONNECTSTATE_CLASS State;
    }
}