using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.UI.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Game.Rulesets.Touhosu.UI.HUD;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class TouhosuPlayfield : Playfield
    {
        public static readonly Vector2 BASE_SIZE = new Vector2(512, 384);
        public static readonly Vector2 ACTUAL_SIZE = new Vector2(307, 384);
        public static readonly float X_SCALE_MULTIPLIER = 0.6f;

        internal readonly TouhosuPlayer Player;

        public TouhosuPlayfield(TouhosuRuleset ruleset)
        {
            InternalChildren = new Drawable[]
            {
                new TouhosuBackground(),
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(X_SCALE_MULTIPLIER, 1),
                    Masking = true,
                    Children = new Drawable[]
                    {
                        Player = new TouhosuPlayer(),
                        HitObjectContainer
                    }
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1 - X_SCALE_MULTIPLIER, 1),
                    RelativePositionAxes = Axes.Both,
                    X = X_SCALE_MULTIPLIER,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Gray
                        },
                        new FillFlowContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 5),
                            Children = new Drawable[]
                            {
                                new HealthDisplay(ruleset.HealthProcessor),
                                new ScoreDisplay(ruleset.ScoreProcessor)
                            }
                        }
                    }
                }
            };
        }

        public override void Add(DrawableHitObject h)
        {
            if (h is DrawableMovingBullet drawable)
            {
                drawable.GetPlayerToTrace(Player);
                base.Add(drawable);
                return;
            }

            base.Add(h);
        }
    }
}
