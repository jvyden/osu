// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Tests.Beatmaps;

namespace osu.Game.Tests.Visual.Gameplay
{
    [TestFixture]
    public partial class TestSceneStrainGraphScene : OsuTestScene
    {
        private Ruleset ruleset = null!;
        private OsuDifficultyCalculator calculator = null!;
        private Dictionary<StrainSkill, List<double>> strainGraphs = null!;
        private FillFlowContainer container = null!;

        [SetUpSteps]
        public void SetUpSteps()
        {
            AddStep("create graph", () => Child = new OsuScrollContainer()
            {
                Child = container = new FillFlowContainer()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    // Size = new Vector2(300, 150),
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                },
                RelativeSizeAxes = Axes.Both,
            });
            AddStep("create difficulty calculator", () =>
            {
                ruleset = new OsuRuleset();
                calculator = (OsuDifficultyCalculator)ruleset.CreateDifficultyCalculator(new TestWorkingBeatmap(new TestBeatmap(ruleset.RulesetInfo)));
            });
        }

        [Test]
        public void TestStrainGraph()
        {
            AddStep("calculate strain", () =>
            {
                strainGraphs = calculator.CalculateStrain();
            });
            AddAssert("has graphs", () => strainGraphs.Count != 0);
            AddAssert("any graph has value", () => strainGraphs.Sum(g => g.Value.Count) != 0);
            AddStep("add graphs", () =>
            {
                foreach ((StrainSkill skill, List<double> list) in strainGraphs)
                {
                    LineGraph graph = new LineGraph
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 64,
                    };

                    container.Add(new OsuSpriteText
                    {
                        Text = skill.GetType().Name,
                        // RelativeSizeAxes = Axes.Both,
                    });

                    container.Add(graph);

                    graph.Values = list.Select(v => (float)v);
                }
            });
        }
    }
}
