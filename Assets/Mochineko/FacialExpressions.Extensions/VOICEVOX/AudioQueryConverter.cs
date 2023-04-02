#nullable enable
using System.Collections.Generic;
using Mochineko.FacialExpressions.LipSync;
using Mochineko.VOICEVOX_API.QueryCreation;

namespace Mochineko.FacialExpressions.Extensions.VOICEVOX
{
    public static class AudioQueryConverter
    {
        private static readonly IReadOnlyDictionary<string, Viseme> VisemeMap
            = new Dictionary<string, Viseme>
            {
                // Silences
                ["pau"] = Viseme.sil, ["cl"] = Viseme.sil, ["y"] = Viseme.sil,
                // Vowels
                ["a"] = Viseme.aa, ["A"] = Viseme.aa,
                ["i"] = Viseme.ih, ["I"] = Viseme.ih,
                ["u"] = Viseme.ou, ["U"] = Viseme.ou,
                ["e"] = Viseme.E, ["E"] = Viseme.E,
                ["o"] = Viseme.oh, ["O"] = Viseme.oh,
                // Consonants
                ["p"] = Viseme.PP, ["py"] = Viseme.PP, ["b"] = Viseme.PP, ["by"] = Viseme.PP, ["m"] = Viseme.PP,
                ["my"] = Viseme.PP,
                ["f"] = Viseme.FF, ["v"] = Viseme.FF, ["w"] = Viseme.FF, ["h"] = Viseme.FF, ["hy"] = Viseme.FF,
                ["d"] = Viseme.DD, ["dy"] = Viseme.DD, ["t"] = Viseme.DD, ["ty"] = Viseme.DD, ["ts"] = Viseme.DD,
                ["k"] = Viseme.kk, ["kw"] = Viseme.kk, ["ky"] = Viseme.kk, ["g"] = Viseme.kk, ["gy"] = Viseme.kk,
                ["ch"] = Viseme.CH, ["sh"] = Viseme.CH, ["j"] = Viseme.CH,
                ["z"] = Viseme.SS, ["s"] = Viseme.SS,
                ["N"] = Viseme.nn, ["n"] = Viseme.nn, ["ny"] = Viseme.nn,
                ["r"] = Viseme.RR, ["ry"] = Viseme.RR,
            };

        public static IEnumerable<LipAnimationFrame> ConvertToSequentialAnimationFrames(
            AudioQuery audioQuery)
        {
            var frames = new List<LipAnimationFrame>();

            frames.Add(new LipAnimationFrame(
                new LipSample(Viseme.sil, weight: 0f),
                audioQuery.PrePhonemeLength * audioQuery.SpeedScale));

            foreach (var phase in audioQuery.AccentPhases)
            {
                foreach (var mora in phase.Moras)
                {
                    frames.Add(new LipAnimationFrame(
                        new LipSample(VisemeMap[mora.Vowel], weight: 1f),
                        mora.VowelLength + mora.ConsonantLength ?? 0f));

                    frames.Add(new LipAnimationFrame(
                        new LipSample(VisemeMap[mora.Vowel], weight: 0f),
                        durationSeconds: 0f));
                }

                if (phase.PauseMora is not null)
                {
                    frames.Add(new LipAnimationFrame(
                        new LipSample(Viseme.sil, weight: 0f),
                        durationSeconds: phase.PauseMora.VowelLength * audioQuery.SpeedScale));
                }
            }

            frames.Add(new LipAnimationFrame(
                new LipSample(Viseme.sil, weight: 0f),
                audioQuery.PostPhonemeLength * audioQuery.SpeedScale));

            return frames;
        }
    }
}