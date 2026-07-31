window.questLogParticles = {
  boom: function (x, y) {
    var accent =
      getComputedStyle(document.documentElement)
        .getPropertyValue("--color-accent")
        .trim() || "#10b981";

    var count = 30; // fewer particles than the original 30-50

    for (let i = 0; i < count; i++) {
      let angle = Math.random() * Math.PI * 2;
      let distance = 50 + Math.random() * 150;
      let dx = Math.cos(angle) * distance;
      let dy = Math.sin(angle) * distance;
      let size = 7 + Math.random() * 4;
      let duration = 750 + Math.random() * 400; // faster than the original 1000-2700ms

      let particle = document.createElement("span");
      particle.className = "ql-particle";
      particle.style.cssText =
        "position:fixed;" +
        "left:" +
        x +
        "px;" +
        "top:" +
        y +
        "px;" +
        "width:" +
        size +
        "px;" +
        "height:" +
        size +
        "px;" +
        "background:" +
        accent +
        ";" +
        "border-radius:50%;" +
        "pointer-events:none;" +
        "z-index:2000;" +
        "opacity:1;" +
        "transform:translate(-50%, -50%) scale(1);" +
        "transition: transform " +
        duration +
        "ms ease-out, opacity " +
        duration +
        "ms ease-out;";

      document.body.appendChild(particle);

      requestAnimationFrame(function () {
        requestAnimationFrame(function () {
          particle.style.transform =
            "translate(calc(-50% + " +
            dx +
            "px), calc(-50% + " +
            dy +
            "px)) scale(0)";
          particle.style.opacity = "0";
        });
      });

      setTimeout(
        (function (p) {
          return function () {
            p.remove();
          };
        })(particle),
        duration + 60,
      );
    }
  },
};
