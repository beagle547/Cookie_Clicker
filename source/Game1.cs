using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Cookie_Clicker {

    public class Game1 : Microsoft.Xna.Framework.Game {

        public static bool muteSFX = false, muteBGM = false;
        public static float volSFX = .5f, volBGM = .8f;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random rng = new Random();

        // music
        List<Song> songList = new List<Song>();

        // sounds
        List<SoundEffect> buttonSounds = new List<SoundEffect>();
        SoundEffect table_tap, cookie_crumble;
        
        // textures
        Texture2D filter_Black, sliderBase;
        List<Texture2D> backroundTextures = new List<Texture2D>();
        List<Texture2D> counterTextures = new List<Texture2D>();
        List<Texture2D> decorationTextures = new List<Texture2D>();
        List<Texture2D> pointerTextures = new List<Texture2D>();
        List<Texture2D> cookieTextures = new List<Texture2D>();
        List<Texture2D> particleTextures = new List<Texture2D>();
        List<Texture2D> buttonTextures = new List<Texture2D>();

        // variables for list management
        int curPointer = 0,
            curBackground = 0;
        bool playingIngameMusic = false;
        List<Rectangle> rect_counterList = new List<Rectangle>();

        // fonts
        SpriteFont FontInfo, FontDis, FontMsg, FontBtn;

        List<Floaty> floaties = new List<Floaty>();
        List<Button> buttons = new List<Button>();
        List<Button> buttons_options = new List<Button>();
        List<CheckBox> checkBox = new List<CheckBox>();
        List<Slider> slider = new List<Slider>();
        ParticleEngine Particles;
        List<Vector2> counterPos = new List<Vector2>();
        Vector2 btn_offset = new Vector2(15, 25);
        Vector2 background_pos = new Vector2(0, 0), screen = new Vector2(1024, 864), str_pos_tl = new Vector2(50, 50), str_pos_bl, cookie_pos1 = new Vector2(50, 250), cookie_pos2 = new Vector2(50, 450), cookie_str_pos1, cookie_str_pos2, pointer_origin, score_str_pos, score_str_pos2, cookies_pos, pause_pos, score_pos, cancel_pos, pauseStrSize, cancelStrSize;
        Vector2 timer_origin, score_origin;
        Vector2[] btn_pos, timer_str_pos = new Vector2[3];
        Color[] color = { Color.Red, Color.Crimson, Color.OrangeRed, Color.DarkOrange, Color.Orange, Color.White, Color.Gray, Color.Green, Color.LimeGreen, Color.Lime, Color.YellowGreen, Color.GreenYellow, Color.Yellow, Color.Gold };
        Cookie[] Cookies = new Cookie[20];
        MouseState mouse_old;
        KeyboardState keyboard_old;
        Rectangle mouse_rect;
        int score = 0, summary = 0, cookie_count = 0, cookie_rng = 1, scolor, tcolor, curTPos, bad_click_points = -10, multiplier = 1;
        float timer, time_ingame = 120f, time = 12f, countdown, counter = 0;
        bool isCookie = false, pause = false, cancel = false, clicked = false, click = false, time_trigger = false;
        string str_score,
            str_score_pre = "Your score: ",
            str_pause = "Game paused..\nPress Space to continue!",
            str_cancel = "Oops! looks like you accidently hit ESC :3\nPress Space to continue!\nOr ESC again if you really want to go back :(",
            info_str_general = "Click on the cookies to gain points.\nBut be careful if you don't hit a cookie you will lose points!",
            info_str_cookiePlain = "Cookies without any topping are only worth a few points.",
            info_str_cookieTop = "Cookies with topping vary in worth depending on the topping they have.",
            info_str_pause = "Use Space to pause or ESC to go back to menu while ingame.";
        string[]
            btn_str = { "Play", "Scoreboard", "How to play", "Options", "Quit", "Credits", "Back" },
            btn_str_opt = { "Fullscreen" },
            credits_str = { "Credits:", "~" };

        public enum colors {

            red,
            darkRed,
            orangeRed,
            darkOrange,
            orange,
            white,
            gray,
            green,
            limeGreen,
            lime,
            yellowGreen,
            greenYellow,
            yellow,
            gold
        }

        public enum gameState {

            menu = -1,
            ingame,
            scoreboard,
            howTo,
            options,
            quit,
            credits,
            back
        }

        gameState currentState = gameState.menu;

        public Game1() {

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)screen.X;
            graphics.PreferredBackBufferHeight = (int)screen.Y;
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize() {

            IsMouseVisible = false;

            countdown = time;

            pointer_origin = new Vector2(2, 26);
            btn_pos = new Vector2[btn_str.Length];

            cookie_str_pos1 = new Vector2(cookie_pos1.X + 140, cookie_pos1.Y + 56);
            cookie_str_pos2 = new Vector2(cookie_pos2.X + 140, cookie_pos2.Y + 56);

            base.Initialize();
        }
        
        protected override void LoadContent() {

            // music
            songList.Add(Content.Load<Song>("music/theStorm")); // main menu
            songList.Add(Content.Load<Song>("music/newGame")); // random ingame music
            songList.Add(Content.Load<Song>("music/crush"));
            playingIngameMusic = false;
            MediaPlayer.Volume = volBGM;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(songList[0]);

            // sounds
            buttonSounds.Add(Content.Load<SoundEffect>("sound/movingTray"));
            buttonSounds.Add(Content.Load<SoundEffect>("sound/click"));
            table_tap = Content.Load<SoundEffect>("sound/tock");
            cookie_crumble = Content.Load<SoundEffect>("sound/crumble");
            SoundEffect.MasterVolume = volSFX;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            // textures
            filter_Black = new Texture2D(GraphicsDevice, (int)screen.X, (int)screen.Y);
            sliderBase = Content.Load<Texture2D>("img/slider");
            counterTextures.Add(Content.Load<Texture2D>("img/clock"));
            counterTextures.Add(Content.Load<Texture2D>("img/score"));

            backroundTextures.Add(Content.Load<Texture2D>("img/background"));

            // order defines value of points
            cookieTextures.Add(Content.Load<Texture2D>("cookies/cookie_base"));
            cookieTextures.Add(Content.Load<Texture2D>("cookies/cookie_cream"));
            cookieTextures.Add(Content.Load<Texture2D>("cookies/cookie_sprinkles"));
            cookieTextures.Add(Content.Load<Texture2D>("cookies/cookie_choc"));
            cookieTextures.Add(Content.Load<Texture2D>("cookies/cookie_nuts"));
            cookieTextures.Add(Content.Load<Texture2D>("cookies/cookie_cherry"));
            cookieTextures.Add(Content.Load<Texture2D>("cookies/cookie_strawberrycream"));

            //decorationTextures.Add(Content.Load<Texture2D>("img/vase"));

            particleTextures.Add(Content.Load<Texture2D>("particle/diamant"));
            particleTextures.Add(Content.Load<Texture2D>("particle/kreis"));
            particleTextures.Add(Content.Load<Texture2D>("particle/stern"));
            Particles = new ParticleEngine(particleTextures);

            buttonTextures.Add(Content.Load<Texture2D>("img/button_tray"));
            buttonTextures.Add(Content.Load<Texture2D>("img/button"));
            buttonTextures.Add(Content.Load<Texture2D>("img/button_hover"));

            pointerTextures.Add(Content.Load<Texture2D>("img/pointer_idle"));
            pointerTextures.Add(Content.Load<Texture2D>("img/pointer_click"));

            // fonts
            FontDis = Content.Load<SpriteFont>("font/FontDisplay");
            FontInfo = Content.Load<SpriteFont>("font/FontInfo");
            FontMsg = Content.Load<SpriteFont>("font/FontMsg");
            FontBtn = Content.Load<SpriteFont>("font/FontBtn");

            // load / initialize other content..
            curBackground = rng.Next(0, backroundTextures.Count);

            counterPos.Add(new Vector2(0, 0));
            counterPos.Add(new Vector2(screen.X - counterTextures[1].Width, 0));

            // timer and string origin / position
            setSOrigin();

            for(int i = 0 ; i < counterTextures.Count ; i++) {
                rect_counterList.Add(new Rectangle((int)counterPos[i].X, (int)counterPos[i].Y, counterTextures[i].Width, counterTextures[i].Height));
            }

            for(int i = 0 ; i < Cookies.Length ; i++) {
                Cookies[i] = new Cookie(rect_counterList, cookieTextures, screen, rng);
            }
            

            for(int i = 0 ; i < btn_pos.Length - 3 ; i++) {
                multiplier = i >= 1 ? 2 : 1;
                btn_pos[i] = new Vector2(btn_offset.X, (buttonTextures[1].Height * i) + btn_offset.Y * multiplier);
                buttons.Add(new Button(buttonTextures, btn_pos[i], FontBtn, btn_str[i], i));
            }

            // Quit
            btn_pos[btn_pos.Length - 3] = new Vector2(btn_offset.X, screen.Y - buttonTextures[1].Height - btn_offset.Y);
            buttons.Add(new Button(buttonTextures, btn_pos[btn_pos.Length - 3], FontBtn, btn_str[btn_pos.Length - 3], (int)gameState.quit));
            // credits
            btn_pos[btn_pos.Length - 2] = new Vector2(screen.X - buttonTextures[1].Width - btn_offset.X, screen.Y - buttonTextures[1].Height - btn_offset.Y);
            buttons.Add(new Button(buttonTextures, btn_pos[btn_pos.Length - 2], FontBtn, btn_str[btn_pos.Length - 2], (int)gameState.credits));
            // back button
            btn_pos[btn_pos.Length - 1] = new Vector2(btn_offset.X, screen.Y - buttonTextures[1].Height - btn_offset.Y);
            buttons.Add(new Button(buttonTextures, btn_pos[btn_pos.Length - 1], FontBtn, btn_str[btn_pos.Length - 1], (int)gameState.back));

            slider.Add(new Slider(sliderBase, cookieTextures, new Vector2(btn_offset.X + 25, btn_offset.Y + 125), FontBtn, "Volume Music", volBGM, rng));
            slider.Add(new Slider(sliderBase, cookieTextures, new Vector2(btn_offset.X + 25, btn_offset.Y + 225), FontBtn, "Volume Sound", volSFX, rng));

            checkBox.Add(new CheckBox(cookieTextures, new Vector2(btn_offset.X + 25, btn_offset.Y + 325), FontBtn, "Mute BGM", rng));
            checkBox.Add(new CheckBox(cookieTextures, new Vector2(btn_offset.X + 25, btn_offset.Y + 425), FontBtn, "Mute SFX", rng));

            str_score = str_score_pre + score.ToString();
            score_pos = FontInfo.MeasureString(str_score);
            score_pos = new Vector2((screen.X / 2) - (score_pos.X / 2), (screen.Y / 2) - (score_pos.Y / 2));

            str_pos_bl = new Vector2(50, screen.Y - 225);

            timer_origin = FontDis.MeasureString(timer.ToString());
            timer_origin = new Vector2(timer_origin.X / 2, timer_origin.Y / 2);
            timer_str_pos[0] = new Vector2(counterPos[0].X + 74, counterPos[0].Y + 110);
            timer_str_pos[1] = new Vector2(counterPos[0].X + 86, counterPos[0].Y + 107);
            timer_str_pos[2] = new Vector2(counterPos[0].X + 98, counterPos[0].Y + 104);
            curTPos = 0;

            pauseStrSize = FontMsg.MeasureString(str_pause);
            pause_pos = new Vector2((screen.X / 2) - (pauseStrSize.X / 2), (screen.Y / 2) - (pauseStrSize.Y / 2));
            cancelStrSize = FontMsg.MeasureString(str_pause);
            cancel_pos = new Vector2((screen.X / 2) - (cancelStrSize.X), (screen.Y / 2) - (cancelStrSize.Y / 2));
            cookies_pos = new Vector2(15, screen.Y - 35);
        }

        private void setSOrigin() {

            score_origin = FontDis.MeasureString(score.ToString());
            score_origin = new Vector2(score_origin.X / 2, score_origin.Y / 2);            
            score_str_pos = new Vector2(counterPos[1].X + 96, counterPos[1].Y + 94);
            score_str_pos2 = new Vector2(counterPos[1].X + 97, counterPos[1].Y + 95);
        }

        protected override void UnloadContent() {
        }
        
        protected override void Update(GameTime gameTime) {
            
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            clicked = false;
            click = false;

            if(mouse.LeftButton == ButtonState.Pressed) {
                click = true;
                curPointer = 1;
            } else
                curPointer = 0;
            
            if(mouse.LeftButton == ButtonState.Pressed && mouse_old.LeftButton == ButtonState.Released) {
                clicked = true;
            }
            mouse_rect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            switch(currentState) {

                default:
                case gameState.back:
                    currentState = gameState.menu;
                break;
                case gameState.menu:
                    if(playingIngameMusic) {
                        playingIngameMusic = false;
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(songList[0]);
                    }
                break;
                case gameState.ingame:
                    setSOrigin();
                    if(!playingIngameMusic) {
                        playingIngameMusic = true;
                        MediaPlayer.IsRepeating = false;
                        MediaPlayer.Play(songList[rng.Next(1, songList.Count)]);
                        setDefaults();
                    }
                    Ingame(gameTime, mouse, keyboard);
                    for(int i = floaties.Count - 1 ; i >= 0 ; --i) {
                        floaties[i].Update();
                        if(!floaties[i].Active)
                            floaties.RemoveAt(i);
                    }
                    if(timer <= 0) {
                        str_score = str_score_pre + score.ToString();
                        score_pos = FontInfo.MeasureString(str_score);
                        score_pos = new Vector2((screen.X / 2) - (score_pos.X / 2), (screen.Y / 2) - (score_pos.Y / 2));
                        currentState = gameState.scoreboard;
                        playingIngameMusic = false;
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(songList[0]);
                }
                break;
                case gameState.scoreboard:
                    
                break;
                case gameState.howTo:
                    if(++counter >= 60) {
                        counter = 0;
                        if(cookie_rng < cookieTextures.Count-1)
                            ++cookie_rng;
                        else
                            cookie_rng = 1;
                    }
                break;
                case gameState.options:
                    slider[0].Update(mouse_rect, buttonSounds[0], click, out volBGM);
                    slider[1].Update(mouse_rect, buttonSounds[0], click, out volSFX);
                    checkBox[0].Update(mouse_rect, cookie_crumble, clicked, out muteBGM);
                    checkBox[1].Update(mouse_rect, cookie_crumble, clicked, out muteSFX);
                    slider[0].setStatus(!muteBGM);
                    slider[1].setStatus(!muteSFX);

                    if(MediaPlayer.Volume != volBGM)
                        MediaPlayer.Volume = volBGM;
                    if(MediaPlayer.IsMuted != muteBGM)
                        MediaPlayer.IsMuted = muteBGM;
                    if(SoundEffect.MasterVolume != volSFX)
                        SoundEffect.MasterVolume = volSFX;
                break;
                case gameState.credits:

                break;
                case gameState.quit:
                    Exit();
                break;
            }

            if(currentState == gameState.menu)
                for(int i = 0 ; i < buttons.Count - 1 ; i++) {
                    currentState = (gameState)buttons[i].Update(mouse_rect, buttonSounds, clicked, (int)currentState);
            } else if(currentState != gameState.ingame) {
                if(currentState == gameState.options)
                    for(int i = 0 ; i < buttons_options.Count ; ++i) {
                        buttons_options[i].Update(mouse_rect, buttonSounds, clicked, 0);
                    }
                currentState = (gameState)buttons[(int)gameState.back].Update(mouse_rect, buttonSounds, clicked, (int)currentState);
            }
            
            keyboard_old = keyboard;
            mouse_old = mouse;

            base.Update(gameTime);

        }

        void Ingame(GameTime gameTime, MouseState mouse, KeyboardState keyboard) {

            if(keyboard.IsKeyDown(Keys.Space) && !keyboard_old.IsKeyDown(Keys.Space)) {
                pause = pause ? false : true;
                cancel = false;
            }
            if(!cancel && (keyboard.IsKeyDown(Keys.Escape) && !keyboard_old.IsKeyDown(Keys.Escape)))
                cancel = true;
            else if(cancel && (keyboard.IsKeyDown(Keys.Escape) && !keyboard_old.IsKeyDown(Keys.Escape))) {
                currentState = gameState.menu;
                cancel = false;
            }

            if(IsActive && !pause && !cancel) {

                if(MediaPlayer.State == MediaState.Paused)
                    MediaPlayer.Resume();

                if(timer > 0) {
                    timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    countdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    tcolor = (int)colors.green;
                    if(timer < 5)
                        tcolor = (int)colors.red;
                    else if(timer < 10) {
                        curTPos = 2;
                        tcolor = (int)colors.darkOrange;
                    } else if(timer < 15)
                        tcolor = (int)colors.orange;
                    else if(timer < 20)
                        tcolor = (int)colors.yellow;
                    else if(timer < 30)
                        tcolor = (int)colors.yellowGreen;
                    else if(timer < 40)
                        tcolor = (int)colors.green;
                    else if(timer < 60)
                        tcolor = (int)colors.limeGreen;
                    else if(timer < 100) {
                        curTPos = 1;
                        tcolor = (int)colors.lime;
                    } else if(timer < 120)
                        tcolor = (int)colors.greenYellow;
                } else {
                    timer = 0;
                }

                time_trigger = false;
                if(countdown <= 0) {
                    time_trigger = true;
                    countdown = time;
                    ++cookie_count;
                }

                if(clicked) {

                    isCookie = false;

                    for(int i = Cookies.Length - cookie_count - 1 ; i >= 0 ; --i) {
                        if(Cookies[i].Active && Cookies[i].UpdateCollision(mouse_rect)) {
                            summary = Cookies[i].GetScore();
                            score += summary;
                            isCookie = true;
                            multiplier = 1;
                            Particles.Active = true;
                            if(!muteSFX)
                                cookie_crumble.Play();
                            floaties.Add(new Floaty(FontInfo, mouse_rect, summary, color[(int)colors.green]));
                            break;
                        }
                    }
                    if(!isCookie) {
                        summary = (int)(score * -.1  + (bad_click_points * multiplier++));
                        if(score + summary < 0)
                            summary = -score;
                        score += summary;
                        if(!muteSFX)
                            table_tap.Play();
                        floaties.Add(new Floaty(FontInfo, mouse_rect, summary, color[(int)colors.darkRed]));
                    }

                    if(score == 0)
                        scolor = (int)colors.white;
                    else if(score < -1000)
                        scolor = (int)colors.darkRed;
                    else if(score < -750)
                        scolor = (int)colors.red;
                    else if(score < -500)
                        scolor = (int)colors.orangeRed;
                    else if(score < -250)
                        scolor = (int)colors.darkOrange;
                    else if(score < 0)
                        scolor = (int)colors.orange;
                    else if(score > 3000)
                        scolor = (int)colors.gold;
                    else if(score > 2000)
                        scolor = (int)colors.greenYellow;
                    else if(score > 1000)
                        scolor = (int)colors.lime;
                    else if(score > 500)
                        scolor = (int)colors.limeGreen;
                    else if(score > 100)
                        scolor = (int)colors.yellowGreen;
                    else if(score > 0)
                        scolor = (int)colors.green;
                }

                for(int i = 0 ; i < Cookies.Length - cookie_count ; ++i) {
                    Cookies[i].Update(gameTime, time_trigger);
                }

                Particles.Update(new Vector2(mouse.X, mouse.Y), rng);

            } else if(cancel) {
                MediaPlayer.Pause();
                pause = true;
            } else {
                MediaPlayer.Pause();
                pause = true;
            }
        }

        private void setDefaults() {
            floaties.Clear();
            pause = false;
            time_trigger = false;
            timer = time_ingame;
            countdown = time;
            multiplier = 1;
            curTPos = 0;
            score = 0;
            scolor = (int)colors.white;
            tcolor = (int)colors.greenYellow;
            for(int i = 0 ; i < Cookies.Length ; i++) {
                Cookies[i].setDefaults();
            }
        }
        
        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(backroundTextures[curBackground], background_pos, Color.White);

            switch(currentState) {

                default:
                case gameState.back:
                case gameState.menu:
                    for(int i = 0 ; i < buttons.Count -1; i++) {
                        buttons[i].Draw(spriteBatch);
                    }
                break;
                case gameState.ingame:
                if(IsActive && !pause) {
                    for(int i = 0 ; i < Cookies.Length - cookie_count ; ++i) {
                        if(Cookies[i].Active)
                            Cookies[i].Draw(spriteBatch);
                    }

                        spriteBatch.Draw(counterTextures[0], counterPos[0], Color.White);
                        spriteBatch.DrawString(FontDis, Math.Round(timer, 2).ToString(), timer_str_pos[curTPos], color[tcolor], -.27f, timer_origin, 1, SpriteEffects.None, 1);
                        spriteBatch.Draw(counterTextures[1], counterPos[1], Color.White);
                        spriteBatch.DrawString(FontDis, score.ToString(), score_str_pos2, Color.Black, .52f, score_origin, 1, SpriteEffects.None, 1);
                        spriteBatch.DrawString(FontDis, score.ToString(), score_str_pos, color[scolor], .52f, score_origin, 1, SpriteEffects.None, 1);
                        //spriteBatch.DrawString(FontInfo, str_cookies + (Cookies.Length - cookie_count).ToString(), cookies_pos, Color.LightBlue);
                        Particles.Draw(spriteBatch);
                        for(int i = 0 ; i < floaties.Count ; ++i) {
                            floaties[i].Draw(spriteBatch);
                        }
                    } else if(cancel) {
                        spriteBatch.DrawString(FontMsg, str_cancel, cancel_pos, Color.Black);
                    } else {
                        spriteBatch.DrawString(FontMsg, str_pause, pause_pos, Color.Black);
                    }
                break;
                case gameState.scoreboard:
                    spriteBatch.DrawString(FontInfo, str_score, score_pos, Color.Black);
                    buttons[(int)gameState.back].Draw(spriteBatch);
                break;
                case gameState.howTo:
                    spriteBatch.DrawString(FontInfo, info_str_general, str_pos_tl, Color.Black);
                    spriteBatch.Draw(cookieTextures[0], cookie_pos1, Color.White);
                    spriteBatch.DrawString(FontInfo, info_str_cookiePlain, cookie_str_pos1, Color.Black);
                    spriteBatch.Draw(cookieTextures[0], cookie_pos2, Color.White);
                    spriteBatch.DrawString(FontInfo, info_str_cookieTop, cookie_str_pos2, Color.Black);
                    spriteBatch.Draw(cookieTextures[cookie_rng], cookie_pos2, Color.White);
                    spriteBatch.DrawString(FontInfo, info_str_pause, str_pos_bl, Color.Black);
                    buttons[(int)gameState.back].Draw(spriteBatch);
                break;
                case gameState.options:
                    spriteBatch.DrawString(FontInfo, "Options", str_pos_tl, Color.Black);
                    foreach(Slider item in slider) {
                        item.Draw(spriteBatch);
                    }
                    foreach(CheckBox item in checkBox) {
                        item.Draw(spriteBatch);
                    }
                    buttons[(int)gameState.back].Draw(spriteBatch);
                break;
                case gameState.credits:
                    spriteBatch.DrawString(FontInfo, "Credits:", str_pos_tl, Color.Black);
                    spriteBatch.DrawString(FontInfo, "\n\nProgramming by Mathias Hille", str_pos_tl, Color.Black);
                    spriteBatch.DrawString(FontInfo, "\n\n\nGraphics by Gabriela Z. D. Santos", str_pos_tl, Color.Black);
                    spriteBatch.DrawString(FontInfo, "\n\n\n\n\nMusic from freemusicarchive.org", str_pos_tl, Color.Black);
                    spriteBatch.DrawString(FontInfo, "\n\n\n\n\n\nCrush by Mystery Mammal, licensed under CC BY\nNew Game by MegaHast3r, licensed under CC BY NC SA\nThrough The Storm Instrumental by Pipe Choir, licensed under CC BY", str_pos_tl, Color.Black);
                    buttons[(int)gameState.back].Draw(spriteBatch);
                break;
                case gameState.quit:
                break;
            }

            spriteBatch.Draw(pointerTextures[curPointer], new Vector2(mouse_old.X, mouse_old.Y), null, Color.White, 0f, pointer_origin, 1, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
