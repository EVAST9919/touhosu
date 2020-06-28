﻿using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModHidden : ModHidden
    {
        public override string Description => @"Play with fading bullets.";
        public override double ScoreMultiplier => 1.06;

        protected override void ApplyHiddenState(DrawableHitObject drawable, ArmedState state)
        {
            if (!(drawable is DrawableMovingBullet))
                return;

            var drawableCherry = (DrawableMovingBullet)drawable;
            drawableCherry.HiddenApplied = true;
        }
    }
}
