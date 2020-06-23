using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Touhosu.UI.Objects;
using osu.Game.Rulesets.UI;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Game.Rulesets.Touhosu.UI.HUD;
using osu.Framework.Allocation;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class TouhosuPlayfield : Playfield
    {
        public static readonly Vector2 BASE_SIZE = new Vector2(512, 384);
        public static readonly Vector2 ACTUAL_SIZE = new Vector2(307, 384);
        public static readonly float X_SCALE_MULTIPLIER = 0.6f;

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        private readonly TouhosuRuleset ruleset;
        private TouhosuPlayer player;

        public TouhosuPlayfield(TouhosuRuleset ruleset)
        {
            this.ruleset = ruleset;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            player = new TouhosuPlayer();
            dependencies.Cache(player);

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
                        player,
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

            player.HitObjects = HitObjectContainer;
        }

        private bool failInvoked;

        protected override void Update()
        {
            base.Update();

            if (!ruleset.HealthProcessor.HasFailed)
                return;

            if (failInvoked)
                return;

            onFail();
            failInvoked = true;
        }

        private void onFail()
        {
            player.Die();
        }
    }
}
