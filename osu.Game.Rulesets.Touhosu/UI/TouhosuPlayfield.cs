﻿using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.UI.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class TouhosuPlayfield : Playfield
    {
        public static readonly Vector2 BASE_SIZE = new Vector2(512, 384);
        public static readonly Vector2 ACTUAL_SIZE = new Vector2(256, 384);

        internal readonly TouhosuPlayer Player;

        public TouhosuPlayfield()
        {
            InternalChildren = new Drawable[]
            {
                new TouhosuBackground(),
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 1),
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
                    Size = new Vector2(0.5f, 1),
                    RelativePositionAxes = Axes.Both,
                    X = 0.5f,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Gray
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
