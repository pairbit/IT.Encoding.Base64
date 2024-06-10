using System;
using System.Runtime.CompilerServices;

namespace IT.Encoding.Base64;

public static class Base64
{
    public static string ToString(ushort encoded) => string.Create(2, encoded, static (chars, encoded) =>
    {
        ref byte b = ref Unsafe.As<ushort, byte>(ref encoded);
        chars[0] = (char)b;
        chars[1] = (char)Unsafe.AddByteOffset(ref b, 1);
    });

    public static string ToString<T>(T encoded) where T : unmanaged => string.Create(Unsafe.SizeOf<T>(), encoded, static (chars, encoded) =>
    {
        ref byte b = ref Unsafe.As<T, byte>(ref encoded);
        chars[0] = (char)b;
        for (int i = 1; i < chars.Length; i++)
        {
            chars[i] = (char)Unsafe.AddByteOffset(ref b, i);
        }
    });

    public static EncodingStatus TryToChars<T>(T encoded, Span<char> chars) where T : unmanaged
    {
        if (chars.Length < Unsafe.SizeOf<T>()) return EncodingStatus.InvalidDestinationLength;

        ref byte b = ref Unsafe.As<T, byte>(ref encoded);
        chars[0] = (char)b;
        for (int i = 1; i < chars.Length; i++)
        {
            chars[i] = (char)Unsafe.AddByteOffset(ref b, i);
        }
        return EncodingStatus.Done;
    }

    public static char[] ToChars<T>(T encoded) where T : unmanaged
    {
        var chars = new char[Unsafe.SizeOf<T>()];
        ref byte b = ref Unsafe.As<T, byte>(ref encoded);
        chars[0] = (char)b;
        for (int i = 1; i < chars.Length; i++)
        {
            chars[i] = (char)Unsafe.AddByteOffset(ref b, i);
        }
        return chars;
    }

    public static bool TryTo<T>(ReadOnlySpan<char> encoded, out T value) where T : unmanaged
    {
        value = default;
        if (Unsafe.SizeOf<T>() != encoded.Length) return false;

        ref byte b = ref Unsafe.As<T, byte>(ref value);

        b = (byte)encoded[0];
        for (int i = 1; i < encoded.Length; i++)
        {
            Unsafe.AddByteOffset(ref b, i) = (byte)encoded[i];
        }

        return true;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T To<T>(ReadOnlySpan<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != encoded.Length) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, $"length != {Unsafe.SizeOf<T>()}");

        T value = default;
        ref byte b = ref Unsafe.As<T, byte>(ref value);

        b = (byte)encoded[0];
        for (int i = 1; i < encoded.Length; i++)
        {
            Unsafe.AddByteOffset(ref b, i) = (byte)encoded[i];
        }

        return value;
    }

    public const string Encoding = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

    public static readonly char[] Chars = Encoding.ToCharArray();

    public static readonly byte[] Bytes = System.Text.Encoding.UTF8.GetBytes(Encoding);

    public static readonly sbyte[] Map = [
    -1, //0
    -1, //1
    -1, //2
    -1, //3
    -1, //4
    -1, //5
    -1, //6
    -1, //7
    -1, //8
    -1, //9
    -1, //10
    -1, //11
    -1, //12
    -1, //13
    -1, //14
    -1, //15
    -1, //16
    -1, //17
    -1, //18
    -1, //19
    -1, //20
    -1, //21
    -1, //22
    -1, //23
    -1, //24
    -1, //25
    -1, //26
    -1, //27
    -1, //28
    -1, //29
    -1, //30
    -1, //31
    -1, //32
    -1, //33
    -1, //34
    -1, //35
    -1, //36
    -1, //37
    -1, //38
    -1, //39
    -1, //40
    -1, //41
    -1, //42
    62, //43 -> +
    -1, //44
    63, //45 -> -
    -1, //46
    -1, //47 -> /
    52, //48 -> 0
    53, //49 -> 1
    54, //50 -> 2
    55, //51 -> 3
    56, //52 -> 4
    57, //53 -> 5
    58, //54 -> 6
    59, //55 -> 7
    60, //56 -> 8
    61, //57 -> 9
    -1, //58
    -1, //59
    -1, //60
    -1, //61
    -1, //62
    -1, //63
    -1, //64
     0, //65 -> A
     1, //66 -> B
     2, //67 -> C
     3, //68 -> D
     4, //69 -> E
     5, //70 -> F
     6, //71 -> G
     7, //72 -> H
     8, //73 -> I
     9, //74 -> J
    10, //75 -> K
    11, //76 -> L
    12, //77 -> M
    13, //78 -> N
    14, //79 -> O
    15, //80 -> P
    16, //81 -> Q
    17, //82 -> R
    18, //83 -> S
    19, //84 -> T
    20, //85 -> U
    21, //86 -> V
    22, //87 -> W
    23, //88 -> X
    24, //89 -> Y
    25, //90 -> Z
    -1, //91
    -1, //92
    -1, //93
    -1, //94
    -1, //95 -> _
    -1, //96
    26, //97 -> a
    27, //98 -> b
    28, //99 -> c
    29, //100 -> d
    30, //101 -> e
    31, //102 -> f
    32, //103 -> g
    33, //104 -> h
    34, //105 -> i
    35, //106 -> j
    36, //107 -> k
    37, //108 -> l
    38, //109 -> m
    39, //110 -> n
    40, //111 -> o
    41, //112 -> p
    42, //113 -> q
    43, //114 -> r
    44, //115 -> s
    45, //116 -> t
    46, //117 -> u
    47, //118 -> v
    48, //119 -> w
    49, //120 -> x
    50, //121 -> y
    51, //122 -> z
    -1, //123
    -1, //124
    -1, //125
    -1, //126
    -1, //127
    -1, //128
    -1, //129
    -1, //130
    -1, //131
    -1, //132
    -1, //133
    -1, //134
    -1, //135
    -1, //136
    -1, //137
    -1, //138
    -1, //139
    -1, //140
    -1, //141
    -1, //142
    -1, //143
    -1, //144
    -1, //145
    -1, //146
    -1, //147
    -1, //148
    -1, //149
    -1, //150
    -1, //151
    -1, //152
    -1, //153
    -1, //154
    -1, //155
    -1, //156
    -1, //157
    -1, //158
    -1, //159
    -1, //160
    -1, //161
    -1, //162
    -1, //163
    -1, //164
    -1, //165
    -1, //166
    -1, //167
    -1, //168
    -1, //169
    -1, //170
    -1, //171
    -1, //172
    -1, //173
    -1, //174
    -1, //175
    -1, //176
    -1, //177
    -1, //178
    -1, //179
    -1, //180
    -1, //181
    -1, //182
    -1, //183
    -1, //184
    -1, //185
    -1, //186
    -1, //187
    -1, //188
    -1, //189
    -1, //190
    -1, //191
    -1, //192
    -1, //193
    -1, //194
    -1, //195
    -1, //196
    -1, //197
    -1, //198
    -1, //199
    -1, //200
    -1, //201
    -1, //202
    -1, //203
    -1, //204
    -1, //205
    -1, //206
    -1, //207
    -1, //208
    -1, //209
    -1, //210
    -1, //211
    -1, //212
    -1, //213
    -1, //214
    -1, //215
    -1, //216
    -1, //217
    -1, //218
    -1, //219
    -1, //220
    -1, //221
    -1, //222
    -1, //223
    -1, //224
    -1, //225
    -1, //226
    -1, //227
    -1, //228
    -1, //229
    -1, //230
    -1, //231
    -1, //232
    -1, //233
    -1, //234
    -1, //235
    -1, //236
    -1, //237
    -1, //238
    -1, //239
    -1, //240
    -1, //241
    -1, //242
    -1, //243
    -1, //244
    -1, //245
    -1, //246
    -1, //247
    -1, //248
    -1, //249
    -1, //250
    -1, //251
    -1, //252
    -1, //253
    -1, //254
    -1, //255
    ];
}