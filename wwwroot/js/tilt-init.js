// VanillaTilt options reference (pass as the 2nd arg to VanillaTilt.init):
// {
//     reverse:                false,  // reverse the tilt direction
//     max:                    35,     // max tilt rotation (degrees)
//     startX:                 0,      // the starting tilt on the X axis, in degrees.
//     startY:                 0,      // the starting tilt on the Y axis, in degrees.
//     perspective:            1000,   // Transform perspective, the lower the more extreme the tilt gets.
//     scale:                  1,      // 2 = 200%, 1.5 = 150%, etc..
//     speed:                  300,    // Speed of the enter/exit transition
//     transition:             true,   // Set a transition on enter/exit.
//     axis:                   null,   // What axis should be enabled. Can be "x" or "y"
//     reset:                  true,   // If the tilt effect has to be reset on exit.
//     "reset-to-start":       true,   // Whether the exit reset will go to [0,0] (default) or [startX, startY]
//     easing:                 "cubic-bezier(.03,.98,.52,.99)",    // Easing on enter/exit.
//     glare:                  false,  // if it should have a "glare" effect
//     "max-glare":            1,      // the maximum "glare" opacity (1 = 100%, 0.5 = 50%)
//     "glare-prerender":      false,  // false = VanillaTilt creates the glare elements for you, otherwise
//                                     // you need to add .js-tilt-glare>.js-tilt-glare-inner by yourself
//     "mouse-event-element":  null,   // css-selector or link to HTML-element what will be listen mouse events
//     gyroscope:              true,   // Boolean to enable/disable device orientation detection,
//     gyroscopeMinAngleX:     -45,    // bottom limit of the device angle on X axis
//     gyroscopeMaxAngleX:     45,     // top limit of the device angle on X axis
//     gyroscopeMinAngleY:     -45,    // bottom limit of the device angle on Y axis
//     gyroscopeMaxAngleY:     45,     // top limit of the device angle on Y axis
// }

window.questLogTilt = {
  init: function () {
    if (!window.VanillaTilt) return;

    VanillaTilt.init(document.querySelectorAll("[data-tilt]"), {
      speed: 500,
      scale: 1.03,
      glare: true,
      "max-glare": 0.2,
    });
  },
};
