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
                ["a"] = Viseme.aa, ["A"] = Viseme.aa,
                ["i"] = Viseme.ih, ["I"] = Viseme.ih,
                ["u"] = Viseme.ou, ["U"] = Viseme.ou,
                ["e"] = Viseme.E, ["E"] = Viseme.E,
                ["o"] = Viseme.oh, ["O"] = Viseme.oh,
                ["pau"] = Viseme.sil, ["N"] = Viseme.nn,
            };

        public static IEnumerable<LipAnimationFrame> Convert(AudioQuery audioQuery)
        {
            var frames = new List<LipAnimationFrame>();

            frames.Add(new LipAnimationFrame(
                new LipSample(Viseme.sil, weight: 0f),
                audioQuery.PrePhonemeLength));

            foreach (var phase in audioQuery.AccentPhases)
            {
                foreach (var mora in phase.Moras)
                {
                    if (mora.ConsonantLength != null)
                    {
                        frames.Add(new LipAnimationFrame(
                            new LipSample(VisemeMap[mora.Vowel], weight: 1f),
                            mora.ConsonantLength.Value));
                    }
                    
                    frames.Add(new LipAnimationFrame(
                        new LipSample(VisemeMap[mora.Vowel], weight: 1f),
                        mora.VowelLength));

                    frames.Add(new LipAnimationFrame(
                        new LipSample(VisemeMap[mora.Vowel], weight: 0f),
                        durationSeconds: 0f));
                }

                if (phase.PauseMora is not null)
                {
                    frames.Add(new LipAnimationFrame(
                        new LipSample(VisemeMap[phase.PauseMora.Vowel], weight: 0f),
                        durationSeconds: phase.PauseMora.VowelLength));
                }
            }

            frames.Add(new LipAnimationFrame(
                new LipSample(Viseme.sil, weight: 0f),
                audioQuery.PostPhonemeLength));

            return frames;
        }
    }
}