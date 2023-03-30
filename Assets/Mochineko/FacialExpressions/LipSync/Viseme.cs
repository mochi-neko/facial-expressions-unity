#nullable enable

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Any of a group of speech sounds that look the same defined by https://visagetechnologies.com/uploads/2012/08/MPEG-4FBAOverview.pdf
    /// </summary>
    // ReSharper disable InconsistentNaming
    public enum Viseme : byte
    {
        sil = 0,
        PP = 1,
        FF = 2,
        TH = 3,
        DD = 4,
        kk = 5,
        CH = 6,
        SS = 7,
        nn = 8,
        RR = 9,
        aa = 10,
        E = 11,
        ih = 12,
        oh = 13,
        ou = 14,
    }
}