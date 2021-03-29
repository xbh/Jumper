using System;

namespace Jumper {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new GameSocket())
                game.Run();
        }
    }
}
