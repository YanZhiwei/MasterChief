using System;

namespace MasterChief.DotNet4.WindowsAPI.Enum
{
    public enum WTS_CONNECTSTATE_CLASS
    {
        WTSActive,
        WTSConnected,
        WTSConnectQuery,
        WTSShadow,
        WTSDisconnected,
        WTSIdle,
        WTSListen,
        WTSReset,
        WTSDown,
        WTSInit
    }

    [Flags]
    public enum WinStationAccess : uint
    {
        WINSTA_NONE = 0,

        WINSTA_ENUMDESKTOPS = 0x0001,
        WINSTA_READATTRIBUTES = 0x0002,
        WINSTA_ACCESSCLIPBOARD = 0x0004,
        WINSTA_CREATEDESKTOP = 0x0008,
        WINSTA_WRITEATTRIBUTES = 0x0010,
        WINSTA_ACCESSGLOBALATOMS = 0x0020,
        WINSTA_EXITWINDOWS = 0x0040,
        WINSTA_ENUMERATE = 0x0100,
        WINSTA_READSCREEN = 0x0200,

        WINSTA_ALL_ACCESS = WINSTA_ENUMDESKTOPS | WINSTA_READATTRIBUTES | WINSTA_ACCESSCLIPBOARD |
                            WINSTA_CREATEDESKTOP | WINSTA_WRITEATTRIBUTES | WINSTA_ACCESSGLOBALATOMS |
                            WINSTA_EXITWINDOWS | WINSTA_ENUMERATE | WINSTA_READSCREEN |
                            StandardAccess.STANDARD_RIGHTS_REQUIRED,

        GENERIC_ALL = StandardAccess.GENERIC_ALL
    }

    public enum WTS_INFO_CLASS
    {
        WTSInitialProgram,
        WTSApplicationName,
        WTSWorkingDirectory,
        WTSOEMId,
        WTSSessionId,
        WTSUserName,
        WTSWinStationName,
        WTSDomainName,
        WTSConnectState,
        WTSClientBuildNumber,
        WTSClientName,
        WTSClientDirectory,
        WTSClientProductId,
        WTSClientHardwareId,
        WTSClientAddress,
        WTSClientDisplay,
        WTSClientProtocolType
    }

    internal enum SysCommands
    {
        SC_SIZE = 0xF000,
        SC_MOVE = 0xF010,
        SC_MINIMIZE = 0xF020,
        SC_MAXIMIZE = 0xF030,
        SC_NEXTWINDOW = 0xF040,
        SC_PREVWINDOW = 0xF050,
        SC_CLOSE = 0xF060,
        SC_VSCROLL = 0xF070,
        SC_HSCROLL = 0xF080,
        SC_MOUSEMENU = 0xF090,
        SC_KEYMENU = 0xF100,
        SC_ARRANGE = 0xF110,
        SC_RESTORE = 0xF120,
        SC_TASKLIST = 0xF130,
        SC_SCREENSAVE = 0xF140,
        SC_HOTKEY = 0xF150,

        //#if(WINVER >= 0x0400) //Win95
        SC_DEFAULT = 0xF160,
        SC_MONITORPOWER = 0xF170,
        SC_CONTEXTHELP = 0xF180,
        SC_SEPARATOR = 0xF00F,
        //#endif /* WINVER >= 0x0400 */

        //#if(WINVER >= 0x0600) //Vista
        SCF_ISSECURE = 0x00000001,
        //#endif /* WINVER >= 0x0600 */

        /*
          * Obsolete names
          */
        SC_ICON = SC_MINIMIZE,
        SC_ZOOM = SC_MAXIMIZE
    }

    internal enum Wm
    {
        WM_ACTIVATE = 0x0006,
        WM_ACTIVATEAPP = 0x001C,
        WM_AFXFIRST = 0x0360,
        WM_AFXLAST = 0x037F,
        WM_APP = 0x8000,
        WM_ASKCBFORMATNAME = 0x030C,
        WM_CANCELJOURNAL = 0x004B,
        WM_CANCELMODE = 0x001F,
        WM_CAPTURECHANGED = 0x0215,
        WM_CHANGECBCHAIN = 0x030D,
        WM_CHANGEUISTATE = 0x0127,
        WM_CHAR = 0x0102,
        WM_CHARTOITEM = 0x002F,
        WM_CHILDACTIVATE = 0x0022,
        WM_CLEAR = 0x0303,
        WM_CLOSE = 0x0010,
        WM_COMMAND = 0x0111,
        WM_COMPACTING = 0x0041,
        WM_COMPAREITEM = 0x0039,
        WM_CONTEXTMENU = 0x007B,
        WM_COPY = 0x0301,
        WM_COPYDATA = 0x004A,
        WM_CREATE = 0x0001,
        WM_CTLCOLORBTN = 0x0135,
        WM_CTLCOLORDLG = 0x0136,
        WM_CTLCOLOREDIT = 0x0133,
        WM_CTLCOLORLISTBOX = 0x0134,
        WM_CTLCOLORMSGBOX = 0x0132,
        WM_CTLCOLORSCROLLBAR = 0x0137,
        WM_CTLCOLORSTATIC = 0x0138,
        WM_CUT = 0x0300,
        WM_DEADCHAR = 0x0103,
        WM_DELETEITEM = 0x002D,
        WM_DESTROY = 0x0002,
        WM_DESTROYCLIPBOARD = 0x0307,
        WM_DEVICECHANGE = 0x0219,
        WM_DEVMODECHANGE = 0x001B,
        WM_DISPLAYCHANGE = 0x007E,
        WM_DRAWCLIPBOARD = 0x0308,
        WM_DRAWITEM = 0x002B,
        WM_DROPFILES = 0x0233,
        WM_ENABLE = 0x000A,
        WM_ENDSESSION = 0x0016,
        WM_ENTERIDLE = 0x0121,
        WM_ENTERMENULOOP = 0x0211,
        WM_ENTERSIZEMOVE = 0x0231,
        WM_ERASEBKGND = 0x0014,
        WM_EXITMENULOOP = 0x0212,
        WM_EXITSIZEMOVE = 0x0232,
        WM_FONTCHANGE = 0x001D,
        WM_GETDLGCODE = 0x0087,
        WM_GETFONT = 0x0031,
        WM_GETHOTKEY = 0x0033,
        WM_GETICON = 0x007F,
        WM_GETMINMAXINFO = 0x0024,
        WM_GETOBJECT = 0x003D,
        WM_GETTEXT = 0x000D,
        WM_GETTEXTLENGTH = 0x000E,
        WM_HANDHELDFIRST = 0x0358,
        WM_HANDHELDLAST = 0x035F,
        WM_HELP = 0x0053,
        WM_HOTKEY = 0x0312,
        WM_HSCROLL = 0x0114,
        WM_HSCROLLCLIPBOARD = 0x030E,
        WM_ICONERASEBKGND = 0x0027,
        WM_IME_CHAR = 0x0286,
        WM_IME_COMPOSITION = 0x010F,
        WM_IME_COMPOSITIONFULL = 0x0284,
        WM_IME_CONTROL = 0x0283,
        WM_IME_ENDCOMPOSITION = 0x010E,
        WM_IME_KEYDOWN = 0x0290,
        WM_IME_KEYLAST = 0x010F,
        WM_IME_KEYUP = 0x0291,
        WM_IME_NOTIFY = 0x0282,
        WM_IME_REQUEST = 0x0288,
        WM_IME_SELECT = 0x0285,
        WM_IME_SETCONTEXT = 0x0281,
        WM_IME_STARTCOMPOSITION = 0x010D,
        WM_INITDIALOG = 0x0110,
        WM_INITMENU = 0x0116,
        WM_INITMENUPOPUP = 0x0117,
        WM_INPUTLANGCHANGE = 0x0051,
        WM_INPUTLANGCHANGEREQUEST = 0x0050,
        WM_KEYDOWN = 0x0100,
        WM_KEYFIRST = 0x0100,
        WM_KEYLAST = 0x0108,
        WM_KEYUP = 0x0101,
        WM_KILLFOCUS = 0x0008,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MBUTTONDBLCLK = 0x0209,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_MDIACTIVATE = 0x0222,
        WM_MDICASCADE = 0x0227,
        WM_MDICREATE = 0x0220,
        WM_MDIDESTROY = 0x0221,
        WM_MDIGETACTIVE = 0x0229,
        WM_MDIICONARRANGE = 0x0228,
        WM_MDIMAXIMIZE = 0x0225,
        WM_MDINEXT = 0x0224,
        WM_MDIREFRESHMENU = 0x0234,
        WM_MDIRESTORE = 0x0223,
        WM_MDISETMENU = 0x0230,
        WM_MDITILE = 0x0226,
        WM_MEASUREITEM = 0x002C,
        WM_MENUCHAR = 0x0120,
        WM_MENUCOMMAND = 0x0126,
        WM_MENUDRAG = 0x0123,
        WM_MENUGETOBJECT = 0x0124,
        WM_MENURBUTTONUP = 0x0122,
        WM_MENUSELECT = 0x011F,
        WM_MOUSEACTIVATE = 0x0021,
        WM_MOUSEFIRST = 0x0200,
        WM_MOUSEHOVER = 0x02A1,
        WM_MOUSELAST = 0x020D,
        WM_MOUSELEAVE = 0x02A3,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_MOUSEHWHEEL = 0x020E,
        WM_MOVE = 0x0003,
        WM_MOVING = 0x0216,
        WM_NCACTIVATE = 0x0086,
        WM_NCCALCSIZE = 0x0083,
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_NCHITTEST = 0x0084,
        WM_NCLBUTTONDBLCLK = 0x00A3,
        WM_NCLBUTTONDOWN = 0x00A1,
        WM_NCLBUTTONUP = 0x00A2,
        WM_NCMBUTTONDBLCLK = 0x00A9,
        WM_NCMBUTTONDOWN = 0x00A7,
        WM_NCMBUTTONUP = 0x00A8,
        WM_NCMOUSEHOVER = 0x02A0,
        WM_NCMOUSELEAVE = 0x02A2,
        WM_NCMOUSEMOVE = 0x00A0,
        WM_NCPAINT = 0x0085,
        WM_NCRBUTTONDBLCLK = 0x00A6,
        WM_NCRBUTTONDOWN = 0x00A4,
        WM_NCRBUTTONUP = 0x00A5,
        WM_NCXBUTTONDBLCLK = 0x00AD,
        WM_NCXBUTTONDOWN = 0x00AB,
        WM_NCXBUTTONUP = 0x00AC,
        WM_NCUAHDRAWCAPTION = 0x00AE,
        WM_NCUAHDRAWFRAME = 0x00AF,
        WM_NEXTDLGCTL = 0x0028,
        WM_NEXTMENU = 0x0213,
        WM_NOTIFY = 0x004E,
        WM_NOTIFYFORMAT = 0x0055,
        WM_NULL = 0x0000,
        WM_PAINT = 0x000F,
        WM_PAINTCLIPBOARD = 0x0309,
        WM_PAINTICON = 0x0026,
        WM_PALETTECHANGED = 0x0311,
        WM_PALETTEISCHANGING = 0x0310,
        WM_PARENTNOTIFY = 0x0210,
        WM_PASTE = 0x0302,
        WM_PENWINFIRST = 0x0380,
        WM_PENWINLAST = 0x038F,
        WM_POWER = 0x0048,
        WM_POWERBROADCAST = 0x0218,
        WM_PRINT = 0x0317,
        WM_PRINTCLIENT = 0x0318,
        WM_QUERYDRAGICON = 0x0037,
        WM_QUERYENDSESSION = 0x0011,
        WM_QUERYNEWPALETTE = 0x030F,
        WM_QUERYOPEN = 0x0013,
        WM_QUEUESYNC = 0x0023,
        WM_QUIT = 0x0012,
        WM_RBUTTONDBLCLK = 0x0206,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_RENDERALLFORMATS = 0x0306,
        WM_RENDERFORMAT = 0x0305,
        WM_SETCURSOR = 0x0020,
        WM_SETFOCUS = 0x0007,
        WM_SETFONT = 0x0030,
        WM_SETHOTKEY = 0x0032,
        WM_SETICON = 0x0080,
        WM_SETREDRAW = 0x000B,
        WM_SETTEXT = 0x000C,
        WM_SETTINGCHANGE = 0x001A,
        WM_SHOWWINDOW = 0x0018,
        WM_SIZE = 0x0005,
        WM_SIZECLIPBOARD = 0x030B,
        WM_SIZING = 0x0214,
        WM_SPOOLERSTATUS = 0x002A,
        WM_STYLECHANGED = 0x007D,
        WM_STYLECHANGING = 0x007C,
        WM_SYNCPAINT = 0x0088,
        WM_SYSCHAR = 0x0106,
        WM_SYSCOLORCHANGE = 0x0015,
        WM_SYSCOMMAND = 0x0112,
        WM_SYSDEADCHAR = 0x0107,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_TCARD = 0x0052,
        WM_TIMECHANGE = 0x001E,
        WM_TIMER = 0x0113,
        WM_UNDO = 0x0304,
        WM_UNINITMENUPOPUP = 0x0125,
        WM_USER = 0x0400,
        WM_USERCHANGED = 0x0054,
        WM_VKEYTOITEM = 0x002E,
        WM_VSCROLL = 0x0115,
        WM_VSCROLLCLIPBOARD = 0x030A,
        WM_WINDOWPOSCHANGED = 0x0047,
        WM_WINDOWPOSCHANGING = 0x0046,
        WM_WININICHANGE = 0x001A,
        WM_XBUTTONDBLCLK = 0x020D,
        WM_XBUTTONDOWN = 0x020B,
        WM_XBUTTONUP = 0x020C
    }

    internal class WA
    {
        public const int WA_INACTIVE = 0;
        public const int WA_ACTIVE = 1;
        public const int WA_CLICKACTIVE = 2;
    }

    internal sealed class VIRTUALKEY
    {
        /*
* Virtual Keys, Standard Set */
        public const int VK_LBUTTON = 0x01;
        public const int VK_RBUTTON = 0x02;
        public const int VK_CANCEL = 0x03;
        public const int VK_MBUTTON = 0x04; /* NOT contiguous with L & RBUTTON */

        //#if(_WIN32_WINNT >= 0x0500)
        public const int VK_XBUTTON1 = 0x05; /* NOT contiguous with L & RBUTTON */

        public const int VK_XBUTTON2 = 0x06; /* NOT contiguous with L & RBUTTON */
        //#endif /* _WIN32_WINNT >= 0x0500 */

        /*
    * 0x07 : unassigned */
        public const int VK_BACK = 0x08;
        public const int VK_TAB = 0x09;

        /*
    * 0x0A - 0x0B : reserved */
        public const int VK_CLEAR = 0x0C;
        public const int VK_RETURN = 0x0D;

        public const int VK_SHIFT = 0x10;
        public const int VK_CONTROL = 0x11;
        public const int VK_MENU = 0x12;
        public const int VK_PAUSE = 0x13;
        public const int VK_CAPITAL = 0x14;

        public const int VK_KANA = 0x15;
        public const int VK_HANGEUL = 0x15; /* old name - should be here for compatibility */
        public const int VK_HANGUL = 0x15;
        public const int VK_JUNJA = 0x17;
        public const int VK_FINAL = 0x18;
        public const int VK_HANJA = 0x19;
        public const int VK_KANJI = 0x19;

        public const int VK_ESCAPE = 0x1B;

        public const int VK_CONVERT = 0x1C;
        public const int VK_NONCONVERT = 0x1D;
        public const int VK_ACCEPT = 0x1E;
        public const int VK_MODECHANGE = 0x1F;

        public const int VK_SPACE = 0x20;
        public const int VK_PRIOR = 0x21;
        public const int VK_NEXT = 0x22;
        public const int VK_END = 0x23;
        public const int VK_HOME = 0x24;
        public const int VK_LEFT = 0x25;
        public const int VK_UP = 0x26;
        public const int VK_RIGHT = 0x27;
        public const int VK_DOWN = 0x28;
        public const int VK_SELECT = 0x29;
        public const int VK_PRINT = 0x2A;
        public const int VK_EXECUTE = 0x2B;
        public const int VK_SNAPSHOT = 0x2C;
        public const int VK_INSERT = 0x2D;
        public const int VK_DELETE = 0x2E;
        public const int VK_HELP = 0x2F;

        /*
        public const int VK_LWIN = 0x5B;CII '0' - '9' (0x30 - 0x39)
    * 0x40 : unassigned * VK_A - VK_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A) */
        public const int VK_LWIN = 0x5B;
        public const int VK_RWIN = 0x5C;
        public const int VK_APPS = 0x5D;

        /*
    * 0x5E : reserved */
        public const int VK_SLEEP = 0x5F;

        public const int VK_NUMPAD0 = 0x60;
        public const int VK_NUMPAD1 = 0x61;
        public const int VK_NUMPAD2 = 0x62;
        public const int VK_NUMPAD3 = 0x63;
        public const int VK_NUMPAD4 = 0x64;
        public const int VK_NUMPAD5 = 0x65;
        public const int VK_NUMPAD6 = 0x66;
        public const int VK_NUMPAD7 = 0x67;
        public const int VK_NUMPAD8 = 0x68;
        public const int VK_NUMPAD9 = 0x69;
        public const int VK_MULTIPLY = 0x6A;
        public const int VK_ADD = 0x6B;
        public const int VK_SEPARATOR = 0x6C;
        public const int VK_SUBTRACT = 0x6D;
        public const int VK_DECIMAL = 0x6E;
        public const int VK_DIVIDE = 0x6F;
        public const int VK_F1 = 0x70;
        public const int VK_F2 = 0x71;
        public const int VK_F3 = 0x72;
        public const int VK_F4 = 0x73;
        public const int VK_F5 = 0x74;
        public const int VK_F6 = 0x75;
        public const int VK_F7 = 0x76;
        public const int VK_F8 = 0x77;
        public const int VK_F9 = 0x78;
        public const int VK_F10 = 0x79;
        public const int VK_F11 = 0x7A;
        public const int VK_F12 = 0x7B;
        public const int VK_F13 = 0x7C;
        public const int VK_F14 = 0x7D;
        public const int VK_F15 = 0x7E;
        public const int VK_F16 = 0x7F;
        public const int VK_F17 = 0x80;
        public const int VK_F18 = 0x81;
        public const int VK_F19 = 0x82;
        public const int VK_F20 = 0x83;
        public const int VK_F21 = 0x84;
        public const int VK_F22 = 0x85;
        public const int VK_F23 = 0x86;
        public const int VK_F24 = 0x87;

        /*
    * 0x88 - 0x8F : unassigned */
        public const int VK_NUMLOCK = 0x90;
        public const int VK_SCROLL = 0x91;

        /*
    * NEC PC-9800 kbd definitions */
        public const int VK_OEM_NEC_EQUAL = 0x92; // '=' key on numpad

        /*
    * Fujitsu/OASYS kbd definitions */
        public const int VK_OEM_FJ_JISHO = 0x92; // 'Dictionary' key
        public const int VK_OEM_FJ_MASSHOU = 0x93; // 'Unregister word' key
        public const int VK_OEM_FJ_TOUROKU = 0x94; // 'Register word' key
        public const int VK_OEM_FJ_LOYA = 0x95; // 'Left OYAYUBI' key
        public const int VK_OEM_FJ_ROYA = 0x96; // 'Right OYAYUBI' key

        /*
    * 0x97 - 0x9F : unassigned */
        /*
    * VK_L* & VK_R* - left and right Alt, Ctrl and Shift virtual keys. * Used only as parameters to GetAsyncKeyState() and GetKeyState(). * No other API or message will distinguish left and right keys in this way. */
        public const int VK_LSHIFT = 0xA0;
        public const int VK_RSHIFT = 0xA1;
        public const int VK_LCONTROL = 0xA2;
        public const int VK_RCONTROL = 0xA3;
        public const int VK_LMENU = 0xA4;
        public const int VK_RMENU = 0xA5;

        //#if(_WIN32_WINNT >= 0x0500)
        public const int VK_BROWSER_BACK = 0xA6;
        public const int VK_BROWSER_FORWARD = 0xA7;
        public const int VK_BROWSER_REFRESH = 0xA8;
        public const int VK_BROWSER_STOP = 0xA9;
        public const int VK_BROWSER_SEARCH = 0xAA;
        public const int VK_BROWSER_FAVORITES = 0xAB;
        public const int VK_BROWSER_HOME = 0xAC;

        public const int VK_VOLUME_MUTE = 0xAD;
        public const int VK_VOLUME_DOWN = 0xAE;
        public const int VK_VOLUME_UP = 0xAF;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;
        public const int VK_MEDIA_STOP = 0xB2;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_LAUNCH_MAIL = 0xB4;
        public const int VK_LAUNCH_MEDIA_SELECT = 0xB5;
        public const int VK_LAUNCH_APP1 = 0xB6;
        public const int VK_LAUNCH_APP2 = 0xB7;

        //#endif /* _WIN32_WINNT >= 0x0500 */

        /*
    * 0xB8 - 0xB9 : reserved */
        public const int VK_OEM_1 = 0xBA; // ';:' for US
        public const int VK_OEM_PLUS = 0xBB; // '+' any country
        public const int VK_OEM_COMMA = 0xBC; // ',' any country
        public const int VK_OEM_MINUS = 0xBD; // '-' any country
        public const int VK_OEM_PERIOD = 0xBE; // '.' any country
        public const int VK_OEM_2 = 0xBF; // '/?' for US
        public const int VK_OEM_3 = 0xC0; // '`~' for US

        /*
    * 0xC1 - 0xD7 : reserved */
        /*
    * 0xD8 - 0xDA : unassigned */
        public const int VK_OEM_4 = 0xDB; //  '[{' for US
        public const int VK_OEM_5 = 0xDC; //  '\|' for US
        public const int VK_OEM_6 = 0xDD; //  ']}' for US
        public const int VK_OEM_7 = 0xDE; //  ''"' for US
        public const int VK_OEM_8 = 0xDF;

        /*
    * 0xE0 : reserved */
        /*
    * Various extended or enhanced keyboards */
        public const int VK_OEM_AX = 0xE1; //  'AX' key on Japanese AX kbd
        public const int VK_OEM_102 = 0xE2; //  "<>" or "\|" on RT 102-key kbd.
        public const int VK_ICO_HELP = 0xE3; //  Help key on ICO
        public const int VK_ICO_00 = 0xE4; //  00 key on ICO

        //#if(WINVER >= 0x0400)
        public const int VK_PROCESSKEY = 0xE5;
        //#endif /* WINVER >= 0x0400 */

        public const int VK_ICO_CLEAR = 0xE6;

        //#if(_WIN32_WINNT >= 0x0500)
        public const int VK_PACKET = 0xE7;
        //#endif /* _WIN32_WINNT >= 0x0500 */

        /*
    * 0xE8 : unassigned */
        /*
    * Nokia/Ericsson definitions */
        public const int VK_OEM_RESET = 0xE9;
        public const int VK_OEM_JUMP = 0xEA;
        public const int VK_OEM_PA1 = 0xEB;
        public const int VK_OEM_PA2 = 0xEC;
        public const int VK_OEM_PA3 = 0xED;
        public const int VK_OEM_WSCTRL = 0xEE;
        public const int VK_OEM_CUSEL = 0xEF;
        public const int VK_OEM_ATTN = 0xF0;
        public const int VK_OEM_FINISH = 0xF1;
        public const int VK_OEM_COPY = 0xF2;
        public const int VK_OEM_AUTO = 0xF3;
        public const int VK_OEM_ENLW = 0xF4;
        public const int VK_OEM_BACKTAB = 0xF5;

        public const int VK_ATTN = 0xF6;
        public const int VK_CRSEL = 0xF7;
        public const int VK_EXSEL = 0xF8;
        public const int VK_EREOF = 0xF9;
        public const int VK_PLAY = 0xFA;
        public const int VK_ZOOM = 0xFB;
        public const int VK_NONAME = 0xFC;
        public const int VK_PA1 = 0xFD;
        public const int VK_OEM_CLEAR = 0xFE;

        /*
    * 0xFF : reserved */
        /* missing letters and numbers for convenience*/
        public static int VK_0 = 0x30;
        public static int VK_1 = 0x31;
        public static int VK_2 = 0x32;
        public static int VK_3 = 0x33;
        public static int VK_4 = 0x34;
        public static int VK_5 = 0x35;
        public static int VK_6 = 0x36;
        public static int VK_7 = 0x37;
        public static int VK_8 = 0x38;

        public static int VK_9 = 0x39;

        /* 0x40 : unassigned*/
        public static int VK_A = 0x41;
        public static int VK_B = 0x42;
        public static int VK_C = 0x43;
        public static int VK_D = 0x44;
        public static int VK_E = 0x45;
        public static int VK_F = 0x46;
        public static int VK_G = 0x47;
        public static int VK_H = 0x48;
        public static int VK_I = 0x49;
        public static int VK_J = 0x4A;
        public static int VK_K = 0x4B;
        public static int VK_L = 0x4C;
        public static int VK_M = 0x4D;
        public static int VK_N = 0x4E;
        public static int VK_O = 0x4F;
        public static int VK_P = 0x50;
        public static int VK_Q = 0x51;
        public static int VK_R = 0x52;
        public static int VK_S = 0x53;
        public static int VK_T = 0x54;
        public static int VK_U = 0x55;
        public static int VK_V = 0x56;
        public static int VK_W = 0x57;
        public static int VK_X = 0x58;
        public static int VK_Y = 0x59;
        public static int VK_Z = 0x5A;
    }
}