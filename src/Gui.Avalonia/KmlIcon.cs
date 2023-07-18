// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia.Media.Imaging;

using System.Buffers.Text;
using System.Collections.Immutable;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MMKiwi.PicMapper.Gui.Avalonia;
public class KmlIcon
{
    public string? Key { get; set; }
    public Bitmap? Bitmap { get; set; }

    public override string ToString() => Key ?? "";

    public ReadOnlyMemory<byte> ToDataUri()
    {
        if (Bitmap == null)
            return ReadOnlyMemory<byte>.Empty;

        using MemoryStream ms = new();
        Bitmap?.Save(ms);

        int numBytes = Base64.GetMaxEncodedToUtf8Length((int)ms.Length);

        byte[] dataUriUtf8 = new byte[numBytes + 22];
        DataUriPrefix.CopyTo(dataUriUtf8);

        Base64.EncodeToUtf8(ms.ToArray(), dataUriUtf8.AsSpan(22..), out int bytesConsumed, out int bytesWritten);

        if (bytesConsumed != ms.Length || bytesWritten != numBytes)
            throw new InvalidDataException("OMG");

        return new(dataUriUtf8);
    }

    private static ReadOnlySpan<byte> DataUriPrefix => "data:image/png;base64,"u8;

    public double Height => Bitmap?.Size.Height ?? 16;
    public double Width => Bitmap?.Size.Width ?? 16;
}

public class KmlIconCollection : List<KmlIcon> { }
