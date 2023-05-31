/***********************************************************************
* DESCRIPTION :
*
*
* NOTES :
* 
* 
* WARNINGS :
* 
* 
* OPTIMIZE IMPORTS : NO
* EXCEPTION CONTROL : NO
* DISPOSE CONTROL : YES
* 
*
* AUTHOR :
*
*
* CHANGES :
*
*
*/

using Microsoft.Xna.Framework;
using ShapesAndColorsChallenge.Class.Animated;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.D2;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Web;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowGame : Window, IDisposable
    {
        #region CONST

        readonly Rectangle counterRectangle = BaseBounds.Bounds.RescaleRectangle(0.12f);

        /// <summary>
        /// Tiempo en milisegundos disponible por el usuario para ver la parrilla de fichas.
        /// </summary>
        const int MEMORY_TIME = 3000;

        /// <summary>
        /// Tiempo en milisegundos que estará disponible la ficha maestra.
        /// </summary>
        const int BLINK_TIME = 200;

        /// <summary>
        /// Velocidad de rotación de las fichas en modo rotación, a mayor valor menor es la velocidad de rotación.
        /// </summary>
        const int ROTATION_SPEED = 100;

        /// <summary>
        /// Para que suene un feedback positivo el usuario tiene que encontrar una ficha en menos de este tiempo en milisegundos.
        /// </summary>
        const int TIME_FOR_POSITIVE_FEEDBACK = 600;

        const int REVEAL_PERK_BOUNDS_OFFSET = 8;

        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS        

        /// <summary>
        /// Obtenemos el separador de decimales para la cultura actual.
        /// </summary>
        readonly string decimalSeparator = System.Text.RegularExpressions.Regex.Replace(1.1f.ToStringCulture(), @"\d+", "");

        string remainingTime = string.Empty;
        TimeSpan lastPass = TimeSpan.Zero;
        Button buttonPause;
        WindowPause windowPause;
        AnimationShake shakeAnimationMaster;
        AnimationShake shakeAnimationTile;
        List<AnimationRotate> rotateAnimationTiles = new();
        BackgroundWorker backgroundWorkerMemoryMode;
        WindowResult windowResult;

        /// <summary>
        /// Almacena si se ha reproducido el sonido del contador descendente.
        /// </summary>
        bool[] counterPlayed = new bool[3] { false, false, false };

        /// <summary>
        /// Ubicación y tamaño del botón volver en la pantalla de juego.
        /// </summary>
        Rectangle buttonPauseBounds = new Rectangle(BaseBounds.Limits.X, 350 + BaseBounds.TileSize.Height - BaseBounds.Button.Height, BaseBounds.Button.Width, BaseBounds.Button.Height);

        static Rectangle masterTileBounds = new Rectangle(BaseBounds.Bounds.Width.Half() - 128, 350, BaseBounds.TileSize.Width, BaseBounds.TileSize.Height);

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Parrilla con las fichas.
        /// </summary>
        Grid Grid { get; set; }

        /// <summary>
        /// Barra de progreso de la puntuación.
        /// </summary>
        ProgressBarStars ProgressBar { get; set; }

        /// <summary>
        /// Panel con los potenciadores.
        /// </summary>
        PerksPanel PerksPanel { get; set; }

        /// <summary>
        /// Panel de la puntuación.
        /// </summary>
        PointsPanel PointsPanel { get; set; }

        /// <summary>
        /// Contador de tiempo.
        /// </summary>
        Stopwatch Stopwatch { get; set; } = new Stopwatch();

        /// <summary>
        /// Indica si se está mostrando el contador inicial previo a la partida.
        /// </summary>
        bool ShowingPreCounter { get; set; } = true;

        /// <summary>
        /// Servirá para salir del hilo de forma segura.
        /// </summary>
        List<string> EndThreadTokens { get; set; } = new List<string>();

        /// <summary>
        /// Token actual para cancelar el hilo.
        /// </summary>
        string CurrentEndThreadToken { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de fichas maestras que han aparecido.
        /// </summary>
        int TilesCounter { get; set; } = 0;

        /// <summary>
        /// Puntos totales obtenidos por el jugador.
        /// </summary>
        long Points { get; set; } = 0;

        /// <summary>
        /// Tiempo en milisegundos restante para encontrar la ficha maestra en el grid.
        /// </summary>
        int RemainingTimeCurrentTile { get; set; } = 0;

        /// <summary>
        /// Indica que la ventana se está anulando.
        /// </summary>
        bool Disposing { get; set; } = false;

        /// <summary>
        /// Ficha maestra.
        /// </summary>
        Tile MasterTile { get; set; } = new();

        /// <summary>
        /// Imagen de la ficha maestra.
        /// </summary>
        Image MasterTileImage { get; set; } = new Image(ModalLevel.Window, masterTileBounds, TextureManager.WhitePixel);

        /// <summary>
        /// Indica si se ha comenzado a jugar.
        /// </summary>
        bool Playing { get; set; } = false;

        /// <summary>
        /// Indica que el juego se ha acabado.
        /// </summary>
        bool GameEnded { get; set; } = false;

        /// <summary>
        /// Etiqueta del tiempo restante de la ficha actual.
        /// </summary>
        Label RemainingTimeLabel { get; set; } = new Label(ModalLevel.Window, new Rectangle(0, 0, 1, 1), string.Empty, ColorManager.HardGray, ColorManager.HardGray);

        Rectangle RemainingTimeLabelBounds { get; set; }

        /// <summary>
        /// Etiqueta del contador de fichas.
        /// </summary>
        Label RemainingTilesLabel { get; set; } = new Label(ModalLevel.Window, new Rectangle(0, 0, 1, 1), string.Empty, ColorManager.HardGray, ColorManager.HardGray);

        Rectangle RemainingTilesLabelBounds { get; set; }

        /// <summary>
        /// Indica si el juego está pausado.
        /// </summary>
        bool Paused { get; set; } = false;

        /// <summary>
        /// Modo de juego.
        /// </summary>
        GameMode GameMode { get; set; }

        /// <summary>
        /// Etapa del juego, se modificará para el modo incremental.
        /// </summary>
        int Stage { get; set; }

        /// <summary>
        /// Nivel del juego, se modificará para el modo incremental.
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// Reto en curso.
        /// </summary>
        Challenge Challenge { get; set; } = null;

        /// <summary>
        /// Indica si se está en el momento del modo memoria que no se ve la ficha maestra y solo la parrilla, justo antes de invertir la visualización, ver la ficha maestra pero no la parrilla.
        /// </summary>
        bool MemoryModePrevisualizating { get; set; }

        /// <summary>
        /// Indica que se está ejecutando un modo contrareloj.
        /// </summary>
        bool TimeTrialModeRunning { get; set; } = false;

        /// <summary>
        /// Indica en milisegundos cuando fue la última ficha encontrada.
        /// </summary>
        DateTime LastTileFindedAt { get; set; }

        /// <summary>
        /// Indica el lugar y el tamaño del resaltado de posición del potenciador "Revelar ficha".
        /// </summary>
        Rectangle RevealPerkBounds { get; set; }

        /// <summary>
        /// Cantidad de fichas encontradas por el usuario.
        /// </summary>
        int TilesFinded { get; set; }

        /// <summary>
        /// Cantidad de fallos que ha cometido el usuario.
        /// </summary>
        int UserMistakes { get; set; }

        /// <summary>
        /// Cantidad de potenciadores usados por el usuario.
        /// </summary>
        int PowerUpsUsed { get; set; }

        #endregion

        #region CONSTRUCTORS

        internal WindowGame()
            : base(ModalLevel.Window, BaseBounds.Bounds, WindowType.Game)
        {
            GameMode = OrchestratorManager.GameMode;
            Stage = OrchestratorManager.StageNumber;
            Level = OrchestratorManager.LevelNumber;
            Challenge = OrchestratorManager.Challenge;
            BlockBack = true;
        }

        #endregion

        #region DESTRUCTOR

        /// <summary>
        /// Variable que indica si se ha destruido el objeto.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Libera todos los recursos.
        /// </summary>
        internal new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera los recursos no administrados que utiliza, y libera los recursos administrados de forma opcional.
        /// </summary>
        /// <param name="disposing">True si se quiere liberar los recursos administrados.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            Disposing = true;

            /*Objetos administrados aquí*/
            if (disposing)
            {
                UnsubscribeEvents();
                Grid.Dispose();
                Grid = null;
                ProgressBar.Dispose();
                ProgressBar = null;
                PerksPanel.Dispose();
                PerksPanel = null;
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~WindowGame()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        /// <summary>
        /// Engancha los eventos de los objetos interactivos.
        /// </summary>
        void SubscribeEvents()
        {
            buttonPause.OnClick += ButtonPause_OnClick;
            PerksPanel.OnPerkChangeStart += PerksPanel_OnPerkChangeStart;
            PerksPanel.OnPerkRevealStart += PerksPanel_OnPerkRevealStart;
            PerksPanel.OnPerkTimeStopStart += PerksPanel_OnPerkTimeStopStart;
            PerksPanel.OnPerkTimeStopEnd += PerksPanel_OnPerkTimeStopEnd;
        }

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            buttonPause.OnClick -= ButtonPause_OnClick;
            PerksPanel.OnPerkChangeStart -= PerksPanel_OnPerkChangeStart;
            PerksPanel.OnPerkRevealStart -= PerksPanel_OnPerkRevealStart;
            PerksPanel.OnPerkTimeStopStart -= PerksPanel_OnPerkTimeStopStart;
            PerksPanel.OnPerkTimeStopEnd -= PerksPanel_OnPerkTimeStopEnd;
        }

        private void PerksPanel_OnPerkTimeStopStart(object sender, EventArgs e)
        {
            PowerUpsUsed++;

            if (GameMode.IsTimeTrial())/*El tiempo es el total para todas, se le detiene pero no se reinicia*/
                Stopwatch.Stop();
            else
            {
                Stopwatch.Stop();
                RemainingTimeCurrentTile = GameData.TimeCurrentLevel(Level);
            }
        }

        private void PerksPanel_OnPerkTimeStopEnd(object sender, EventArgs e)
        {
            Stopwatch.Start();
        }

        private void PerksPanel_OnPerkRevealStart(object sender, EventArgs e)
        {
            PowerUpsUsed++;
        }

        private void PerksPanel_OnPerkChangeStart(object sender, EventArgs e)
        {
            PowerUpsUsed++;
            TrySetMasterTile();

            if (!GameMode.IsTimeTrial())/*En contrareloj no se resetea el tiempo ya que volvería al total de todas las fichas*/
            {
                RemainingTimeCurrentTile = GameData.TimeCurrentLevel(Level);
                Stopwatch.Restart();
            }
        }

        internal void Tile_OnClick(object sender, OnClickEventArgs e)
        {
            if (!Playing || GameEnded)
                return;

            if (IsTheCorrectTile(sender as Tile))
                UserCorrect(sender as Tile);
            else/*Se ha equivocado*/
                UserMistake(sender as Tile);
        }

        /// <summary>
        /// No pausa la previsualización del modo memoria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPause_OnClick(object sender, OnClickEventArgs e)
        {
            Stopwatch.Stop();
            Paused = true;
            PerksPanel.Pause(Paused);
            OrchestratorManager.OpenPause(ref windowPause, Challenge != null);
            windowPause.OnResume += WindowPause_OnResume;
            windowPause.OnQuit += WindowPause_OnQuit;
        }

        private void WindowPause_OnResume(object sender, EventArgs e)
        {
            ClosePauseWindow();
            Paused = false;
            PerksPanel.Pause(Paused);

            if (!PerksPanel.IsTimeStopRunning)/*Si está lanzado este potenciador al volver de la pausa no debe iniciar el Stopwatch*/
                Stopwatch.Start();
        }

        private void WindowPause_OnQuit(object sender, EventArgs e)
        {
            ClosePauseWindow();
            CurrentEndThreadToken = EndThreadTokens.Last();/*Esto desbloqueará el hilo de fichas*/
            Paused = false;

            if (GameMode.IsIn(GameMode.Endless, GameMode.EndlessPlus))/*En estos modos se guarda la puntuación*/
                End(true, true);
            else
                End(true, false);
        }

        private void BackgroundWorkerMemoryMode_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalMilliseconds < MEMORY_TIME)
                Thread.SpinWait(1);
        }

        private void BackgroundWorkerMemoryMode_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MasterTileImage.Visible = true;
            Grid.EnableTilesClick();
            Grid.HideTiles();
            SetNextTile();
        }

        private void WindowResult_OnAccept(object sender, EventArgs e)
        {
            WindowManager.Remove(windowResult.ID);
            windowResult = null;
            CloseMeAndOpenThis(OrchestratorManager.GameWindowInvoker);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            ShaderManager.LoadContent();
            SetGrid();
            SetProgressBar();
            SetButtonPause();
            SetPerksPanel();
            SubscribeEvents();
            SetRemainingTimeLabelBounds();
            SetRemainingTilesLabelBounds();
            SetPointsPanel();
            DisableUI();
            InteractiveObjectManager.Add(MasterTileImage, RemainingTimeLabel, RemainingTilesLabel);
            Stopwatch.Start();/*Aquí se inicia para el contador hacía atrás previo*/
        }

        void SetGrid()
        {
            Grid = new Grid(this, ModalLevel);
            Grid.Set(GameMode, Stage, Level);
        }

        void SetProgressBar()
        {
            ProgressBar = new ProgressBarStars(ModalLevel, GameData.MaxPointStageLevel(Level), new Rectangle(BaseBounds.Limits.X, BaseBounds.Limits.Y, BaseBounds.Limits.Width, 100));
            ProgressBar.LoadContent();
        }

        void SetPerksPanel()
        {
            PerksPanel = new(ModalLevel, this);
            PerksPanel.LoadContent();
        }

        void ClosePauseWindow()
        {
            windowPause.OnResume -= WindowPause_OnResume;
            windowPause.OnQuit -= WindowPause_OnQuit;
            WindowManager.Remove(windowPause.ID);
            windowPause = null;
        }

        /// <summary>
        /// Establece el botón de pausa.
        /// </summary>
        void SetButtonPause()
        {
            buttonPause = new(ModalLevel, buttonPauseBounds);
            Image imagePause = new(ModalLevel, buttonPauseBounds, TextureManager.TexturePauseButton, Color.DarkGray, Color.DarkGray, true, 30);
            InteractiveObjectManager.Add(buttonPause, imagePause);
        }

        /// <summary>
        /// Reinicia el paso del juego, el cambio de ficha maestra, etc.
        /// </summary>
        void SetNextTile()
        {
            if (RunMemoryMode())
                return;

            MasterTileImage.Visible = true;/*El modo parpadeo la oculta tras un tiempo, hay que mostrarla cada vez*/
            MemoryModePrevisualizating = false;
            shakeAnimationMaster?.Stop();/*Hay que detener la animación de equivocación*/
            shakeAnimationTile?.Stop();/*Hay que detener la animación de equivocación*/
            TilesCounter++;
            RunMovementMode();
            RunIncrementalMode();/*Si estamos en un modo incremental se sube la dificultad*/
            TrySetMasterTile();
            RunRotateMode();
            StartNextTile();
        }

        void StopAnimations()
        {
            shakeAnimationMaster?.Stop();
            shakeAnimationTile?.Stop();

            for (int i = 0; i < rotateAnimationTiles.Count; i++)
                rotateAnimationTiles[i].Stop();

            rotateAnimationTiles.Clear();
        }

        /// <summary>
        /// Comprueba si la ficha pulsada por el usuario es la correcta.
        /// </summary>
        /// <returns></returns>
        bool IsTheCorrectTile(Tile tile)
        {
            return GameMode.IsPlus() && tile.ShapeType == ShapeType.None
                ? !Grid.IsTileInMe(MasterTile)
                : tile.TileColor == MasterTile.TileColor && tile.ShapeType == MasterTile.ShapeType;
        }

        /// <summary>
        /// El usuario ha acertado.
        /// </summary>
        /// <param name="tile"></param>
        void UserCorrect(Tile tile)
        {
            TilesFinded++;
            StopAnimations();

            if (!GameMode.IsIncremental() || TilesCounter % 3 != 0)/*El resaltado no se muestra en los cambios de incremental ya que la parrilla cambia de tamaño y el resaltado no queda bien*/
                Grid.SetHighLight(tile, true);/*Solo se muestra el resaltado cuando no se va a cambiar la parrilla de tamaño*/

            if (Statics.GetRandom(1, 10) > 5)
                SoundManager.CorrectTile1.PlaySound();
            else
                SoundManager.CorrectTile2.PlaySound();

            SumPoints();
            PlayPositiveFeedback();
            RunningRevealPerk();

            if (GameMode.IsTimeTrial())/*Si es contrareloj solo se puede finalizar por tiempo, no por ficha*/
                SetNextTile();
            else
            {
                CurrentEndThreadToken = EndThreadTokens.Last();
                End();
            }
        }

        /// <summary>
        /// El usuario ha cometido un error.
        /// </summary>
        void UserMistake(Tile tile)
        {
            StopAnimations();
            Statics.Vibrate(VibrationDuration.Short);
            UserMistakes++;

            if (tile != null)/*Si el usuario no ha pulsado en el tiempo límite esta variable será nula*/
            {
                Grid.SetHighLight(tile, false);
                shakeAnimationMaster = new AnimationShake(MasterTileImage, 500);
                shakeAnimationTile = new AnimationShake(tile.Image, 500);
                shakeAnimationMaster.Start();
                shakeAnimationTile.Start();
                SoundManager.WrongTile.PlaySound();
            }

            if (GameMode.IsIncremental())/*Un error en el modo incremental finaliza la partida*/
            {
                CurrentEndThreadToken = EndThreadTokens.Last();
                End(false, false, true);
            }

            if (GameMode.IsPlus())/*Si el modo es plus el error resta puntos, el 10% del tiempo disponible para este nivel*/
                Points = Math.Max(0, Points - (GameData.TimeCurrentLevel(Level) * 0.1f).ToInt());
        }

        void RunningRevealPerk()
        {
            if (!PerksPanel.IsRevealRunning)
                return;

            PerksPanel.RemainingReveal--;

            if (PerksPanel.RemainingReveal.IsZero())
                PerksPanel.IsRevealRunning = false;
        }

        /// <summary>
        /// Lanza la previsualización del modo memoria.
        /// </summary>
        bool RunMemoryMode()
        {
            if (!GameMode.IsMemory() || MemoryModePrevisualizating)/*Si es el modo memoria se secuestra el mostrado de ficha durante x segundos*/
                return false;

            MemoryModePrevisualizating = true;
            MasterTileImage.Visible = false;
            Grid.DisableTilesClick();
            Grid.ShowTiles();
            backgroundWorkerMemoryMode = new() { WorkerSupportsCancellation = false, WorkerReportsProgress = false };
            backgroundWorkerMemoryMode.DoWork += BackgroundWorkerMemoryMode_DoWork;
            backgroundWorkerMemoryMode.RunWorkerCompleted += BackgroundWorkerMemoryMode_RunWorkerCompleted;
            backgroundWorkerMemoryMode.RunWorkerAsync();
            return true;
        }

        /// <summary>
        /// Comienza un hilo con la nueva búsqueda de ficha.
        /// </summary>
        void StartNextTile()
        {
            LastTileFindedAt = DateTime.Now;/*Se usa para controlar la reproducción de sonidos de feedback positivo, es necesario tener el momento inicial*/

            if (TimeTrialModeRunning)/*El modo contrareloj solo ejecuta el hilo una vez*/
                return;

            if (GameMode.IsTimeTrial())/*Para que no se ejecute el hilo*/
                TimeTrialModeRunning = true;

            RemainingTimeCurrentTile = GameData.TimeCurrentLevel(Level);
            Stopwatch = new Stopwatch();

            if (!PerksPanel.IsTimeStopRunning)/*Si está lanzado este potenciador, Stopwatch estará detenido, y así debe seguir hasta que finalice el potenciador*/
                Stopwatch.Restart();

            Thread threadCurrenTile = new(() => ProgressCurrentTile());
            EndThreadTokens.Add(Guid.NewGuid().ToString());
            threadCurrenTile.Start();
        }

        /// <summary>
        /// Si se está en el modo incremental se sube la dificultad progresivamente.
        /// </summary>
        void RunIncrementalMode()
        {
            if (!GameMode.IsIncremental())
                return;

            /*La dificultad aumenta primero por el nivel y cuando ha llegado al máximo cambia de etapa en el nivel más bajo, cada 3 fichas encontradas*/
            /*La dificultad se podría representar como unos dientes de sierra que van ascendiendo cada vez más*/
            if (TilesCounter == 1 || (TilesCounter - 1) % 3 != 0)
                return;

            if (Level < GameData.LEVELS)
            {
                Level++;
                Grid.Set(GameMode, Stage, Level);
            }
            else
            {
                if (Stage < GameData.STAGES)
                {
                    Level = 1;
                    Stage++;
                    Grid.Set(GameMode, Stage, Level);
                }
            }
        }

        void RunMovementMode()
        {
            if (GameMode.IsMovement())
                Grid.Set(GameMode, Stage, Level);
        }

        /// <summary>
        /// Si estamos en modo parpadeo y el tiempo actual de ficha es superior al del modo se oculta la maestra.
        /// La maestra se reestablece siempre en cada nueva ficha.
        /// </summary>
        /// <param name="timeElapsed"></param>
        void RunBlinkMode(long timeElapsed)
        {
            if (GameMode.IsBlink() && timeElapsed > BLINK_TIME)
                MasterTileImage.Visible = false;
        }

        void RunRotateMode()
        {
            if (!GameMode.IsRotate())
                return;

            for (int i = 0; i < rotateAnimationTiles.Count; i++)
                rotateAnimationTiles[i].Stop();

            rotateAnimationTiles.Clear();

            if (MasterTile.ShapeType.CanRotate())
                if (Statics.GetRandom(0, 1).IsZero())/*Rotará si sale 0*/
                    rotateAnimationTiles.Add(new AnimationRotate(MasterTileImage, ROTATION_SPEED, Statics.GetRandom(0, 1).IsZero()));

            for (int i = 0; i < Grid.Tiles.Count; i++)
                if (Grid.Tiles[i].ShapeType.CanRotate())
                    if (Statics.GetRandom(0, 1).IsZero())/*Rotará si sale 0*/
                        rotateAnimationTiles.Add(new AnimationRotate(Grid.Tiles[i].Image, ROTATION_SPEED, Statics.GetRandom(0, 1).IsZero()));

            for (int i = 0; i < rotateAnimationTiles.Count; i++)
                rotateAnimationTiles[i].Start();
        }

        /// <summary>
        /// Suma los puntos obtenidos por encontrar la ficha actual.
        /// </summary>
        void SumPoints()
        {
            if (GameMode.IsTimeTrial())/*En el modo contrareloj la cantidad de puntos a obtener es fija por encontrar una ficha*/
                Points += GameData.GetTimeTrialPointsForFindedTile;
            else
                Points += (RemainingTimeCurrentTile * 1.1f/*10% extra por los reflejos*/).ToInt();
        }

        /// <summary>
        /// Reproduce una voz de ánimo según una condiciones.
        /// </summary>
        void PlayPositiveFeedback()
        {
            double milis = (DateTime.Now - LastTileFindedAt).TotalMilliseconds;

            if (TIME_FOR_POSITIVE_FEEDBACK + ((GameData.HorizontalTilesNumber(Stage, Level) + GameData.VerticalTilesNumber(Stage, Level)) * 20/*Ajustamos según la dificultad*/) >= milis)
                SoundManager.PlayRandomVoicePositiveFeedback();

            LastTileFindedAt = DateTime.Now;
        }

        /// <summary>
        /// Busca la combinación de color y forma para la ficha maestra, en algunos modos de juego debe estar en el grid, en otros no, pero siempre debe ser diferente a la anterior.
        /// </summary>
        void TrySetMasterTile()
        {
            while (true)
            {
                ShapeType shapeType = GameData.RandomShape(Stage, Level, OrchestratorManager.GameMode);
                TileColor tileColor = GameData.RandomColor(Stage, Level);

                if (GameMode.IsPlus())/*Si el modo es plus la ficha no tiene porque estar en la parrilla pero si tiene que ser diferente de la anterior*/
                {
                    if (shapeType == MasterTile.ShapeType && tileColor == MasterTile.TileColor)/*Se comprueba que sea diferente a la anterior*/
                        continue;
                    else
                    {
                        SetMasterTile(shapeType, tileColor);/*Si es diferente se establece*/
                        return;
                    }
                }
                /*Si es un modo normal no debe ser igual a la anterior y debe estar en en la parrilla*/
                if ((shapeType == MasterTile.ShapeType && tileColor == MasterTile.TileColor) || !Grid.IsTileInMe(shapeType, tileColor))
                    continue;

                SetMasterTile(shapeType, tileColor);
                break;
            }
        }

        /// <summary>
        /// Establece la ficha maestra.
        /// </summary>
        /// <param name="shapeType"></param>
        /// <param name="tileColor"></param>
        void SetMasterTile(ShapeType shapeType, TileColor tileColor)
        {
            MasterTile = new Tile(ModalLevel, masterTileBounds, tileColor, shapeType) { EnableOnClick = false };
            MasterTileImage.Origin = Vector2.Zero;
            MasterTileImage.Bounds = masterTileBounds;
            MasterTileImage.Texture = TextureManager.GetShape(shapeType);
            MasterTileImage.ColorLightMode = MasterTileImage.ColorDarkMode = ColorManager.GetShapeColor(tileColor);
            MasterTileImage.EnableOnClick = false;
            MasterTile.Image = MasterTileImage;
        }

        /// <summary>
        /// Lleva la cuenta del tiempo invertido en encontrar la ficha maestra actual en el grid.
        /// Se usa un hilo en vez de usar Update por ser más preciso en cuanto a iteraciones por segundo.
        /// </summary>
        void ProgressCurrentTile()
        {
            while (RemainingTimeCurrentTile > 0)
            {
                if (Disposing || EndThreadTokens.Contains(CurrentEndThreadToken))
                {
                    CurrentEndThreadToken = string.Empty;
                    return;
                }

                if (Paused)
                {
                    Thread.SpinWait(1);
                    continue;
                }

                UpdateProgressCurrentTileRemainingTime();
                Thread.SpinWait(1);
            }

            EndProgressCurrentTile();
        }

        /// <summary>
        /// Actualiza el tiempo restante de la ficha actual.
        /// </summary>
        void UpdateProgressCurrentTileRemainingTime()
        {
            long timeElapsed = Stopwatch.ElapsedMilliseconds;

            if (GameMode.IsTimeTrial())/*En contrareloj el tiempo a descontar es el total de la partida no de la ficha*/
                RemainingTimeCurrentTile = GameData.TimeTrialModeTime(Level).Half()/*Se reduce a la mitad*/ - (int)timeElapsed;
            else
                RemainingTimeCurrentTile = GameData.TimeCurrentLevel(Level) - (int)timeElapsed;

            RunBlinkMode(timeElapsed);
        }

        /// <summary>
        /// Tiene lugar justo al finalizar la búsqueda de la ficha maestra actual.
        /// </summary>
        void EndProgressCurrentTile()
        {
            if (RemainingTimeCurrentTile <= 0)/*Si el tiempo para encontrar la ficha finaliza es como si el usuario huhiera cometido un error.*/
            {
                UserMistake(null);

                if (GameMode.IsIncremental())/*Si el modo es incremental y se ha llegado a cero el usuario ha perdido y se debe finalizar, UserMistake*/
                    return;
            }

            Stopwatch.Stop();
            End();
        }

        /// <summary>
        /// Finaliza el juego o continua si aún no se han completado las fichas a mostrar.
        /// </summary>
        /// <param name="userQuitGame">Indica que el juego debe acabar porque el jugador ha abandonado.</param>
        /// <param name="userQuitButSaveScore">Aunque el jugador haya abandonado se debe guardar el progreso porque puede ser un modo interminable.</param>
        /// <param name="endIncremental">Indica que se ha finalizado el modo incremental.</param>
        void End(bool userQuitGame = false, bool userQuitButSaveScore = false, bool endIncremental = false)
        {
            if (userQuitGame && !userQuitButSaveScore)/*Se ha terminado el juego porque el usuario a abandonado*/
            {
                CloseMeAndOpenThis(OrchestratorManager.GameWindowInvoker);
                Challenge?.ChallengeFailed();/*Si está en un reto directamente lo ha fallado*/
                ChallengesManager.Refresh();/*Añadimos retos si es posible*/
                return;
            }
            else if (userQuitGame && userQuitButSaveScore)/*Se ha terminado el juego porque el usuario a abandonado pero es un modo interminable y entonces sí se guarda la puntuación ya que no hay otra forma de salir*/
            {
                SaveScoreAndProgress();
                return;
            }

            if (endIncremental)
            {
                SaveScoreAndProgress();
                return;
            }

            if (EndGameByTileFinded())/*Se ha acabado el juego porque se han mostrado todas las fichas*/
                SaveScoreAndProgress();
            else if (GameMode.IsTimeTrial())/*Si se ha llegado aquí y es contrareloj es que ya se ha acabado, aquí solo se llega una vez en estos modos*/
                SaveScoreAndProgress();
            else/*Se ha acabado la ficha actual por tiempo o porque se ha encontrado*/
                SetNextTile();
        }

        void SaveScoreAndProgress()
        {
            StopAnimations();
            GameEnded = true;
            Grid.Disable();
            PerksPanel.Disable();
            bool newRecord = UpdateScore();

            if (newRecord)/*Si hay nuevo record intentamos guardar en la nube el total del modo*/
                RestOrchestrator.TryToSaveScore(GameMode);

            int stars = GameData.GetStarsForThisPoints(Points, OrchestratorManager.LevelNumber/*No tiene que ser el level local*/);
            ChallengesManager.Refresh();/*Añadimos retos si es posible*/

            OrchestratorManager.OpenWindowResultMessage(
                ref windowResult,
                new(
                    Points,
                    stars,
                    newRecord,
                    Challenge,
                    Challenge != null,
                    ChallengesManager.IsChallengeCompleted(Challenge, Points, stars, TilesFinded, UserMistakes, PowerUpsUsed)));
            windowResult.OnAccept += WindowResult_OnAccept;
        }

        /// <summary>
        /// Se finalizará el juego por fichas encontradas cuando se hayan mostrado tantas como el nivel tenga y además no se aun modo interminable, incremental o timetrial.
        /// </summary>
        /// <returns></returns>
        bool EndGameByTileFinded()
        {
            return TilesCounter == GameData.TilesCurrenStage && !GameMode.HasUnlimitedTiles();
        }

        /// <summary>
        /// Establece el valor de las propiedades de la etiqueta de tiempo restante.
        /// </summary>
        void SetRemainingTimeLabelBounds()
        {
            Vector2 strinsSize = FontManager.GetFont().MeasureString($"{Resource.String.REMAINING_TIME.GetString()} 0.0");
            int x = BaseBounds.Limits.Right - strinsSize.X.ToInt();
            RemainingTimeLabelBounds = new Rectangle(
                x,
                masterTileBounds.Bottom - 60,
                BaseBounds.Limits.Right - x,
                60);
            RemainingTimeLabel.Bounds = RemainingTimeLabelBounds;
        }

        /// <summary>
        /// Establece el valos de las propiedades de la etiqueta de fichas restantes.
        /// </summary>
        void SetRemainingTilesLabelBounds()
        {
            Vector2 strinsSize = FontManager.GetFont().MeasureString($"{Resource.String.REMAINING_TILES.GetString()} 99/99");
            int x = BaseBounds.Limits.Right - strinsSize.X.ToInt();
            RemainingTilesLabelBounds = new Rectangle(
                x,
                masterTileBounds.Top,
                BaseBounds.Limits.Right - x,
                60);
            RemainingTilesLabel.Bounds = RemainingTilesLabelBounds;
        }

        /// <summary>
        /// Establece las propiedades de la etiqueta de puntos.
        /// </summary>
        void SetPointsPanel()
        {
            PointsPanel = new(ModalLevel, this);
            PointsPanel.LoadContent();
        }

        void DisableUI()
        {
            Grid.Disable();
            PerksPanel.Disable();
            buttonPause.Visible = false;
        }

        void EnableUI()
        {
            Grid.Enable();
            PerksPanel.Enable();
            buttonPause.Visible = true;
        }

        /// <summary>
        /// Almacena la puntuación conseguida si es mejor que la previa, además devuelve True si la conseguida es mejor para mostrar un mensaje de nueva mejor puntuación o similar.
        /// </summary>
        /// <returns></returns>
        bool UpdateScore()
        {
            Score score = ControllerScore.Get(GameMode, OrchestratorManager.StageNumber, OrchestratorManager.LevelNumber);/*Hay que usar los valores de OrchestratorManager, no los locales ya que estos cambian en el modo incremental*/

            if (score.UserScore >= Points)
                return false;/*Si la puntuación guardada es mejor que la conseguida no hacemos nada*/
            else
            {
                score.UserScore = Points;
                score.Stars = GameData.GetStarsForThisPoints(Points, OrchestratorManager.LevelNumber);/*Hay que usar los valores de OrchestratorManager, no los locales ya que estos cambian en el modo incremental*/
                score.TilesFinded = TilesFinded;
                ControllerScore.Update(score);
                return true;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            if (Paused)
                return;

            base.Update(gameTime);
            ProgressBar.SetValue(Points);
            ProgressBar.Update(gameTime);
            PerksPanel.Update(gameTime);

            if (!ShowingPreCounter)
                UpdateGame(gameTime);
        }

        void UpdateGame(GameTime gameTime)
        {
            if (!Playing)/*Se comienza a jugar, esto solo pasa una vez*/
            {
                Playing = true;

                if (GameMode.IsTimeTrial())/*En el modo contrareloj el tiempo no es por ficha, es para todas las que sea capaz de encontrar el usuario*/
                    RemainingTimeCurrentTile = GameData.TimeTrialModeTime(Level).Half();/*Se reduce el tiempo a la mitad*/

                SetNextTile();
                EnableUI();
            }

            UpdateRemainingTileTime();
            UpdateRemainingTiles();
            UpdatePoints(gameTime);
            UpdateAnimations(gameTime);
            UpdateRevealPerk();
            Grid.Update(gameTime);
        }

        void UpdateAnimations(GameTime gameTime)
        {
            shakeAnimationMaster?.Update(gameTime);
            shakeAnimationTile?.Update(gameTime);

            for (int i = 0; i < rotateAnimationTiles.Count; i++)
                rotateAnimationTiles[i].Update(gameTime);
        }

        void UpdateRevealPerk()
        {
            if (!PerksPanel.IsRevealRunning)
                return;

            RevealPerkBounds = Grid.GetThisTileLocation(MasterTile, REVEAL_PERK_BOUNDS_OFFSET.Half());
        }

        internal override void Draw(GameTime gameTime)
        {
            if (ShowingPreCounter)
                DrawPreCounter(gameTime);
            else
            {
                base.Draw(gameTime);
                DrawGame(gameTime);
            }
        }

        void DrawGame(GameTime gameTime)
        {
            Grid.Draw(gameTime);
            ProgressBar.Draw(gameTime);
            PerksPanel.Draw(gameTime);
            PointsPanel.Draw(gameTime);
            DrawMasterTileRectangle();
            DrawRevealPerk(gameTime);
        }

        /// <summary>
        /// Pinta un recuadro alrededor de la ficha maestra.
        /// </summary>
        static void DrawMasterTileRectangle()
        {
            Screen.SpriteBatch.DrawRectangle(masterTileBounds, ColorManager.VeryHardGray, 1f, 1);
        }

        void DrawRevealPerk(GameTime gameTime)
        {
            if (!PerksPanel.IsRevealRunning)
                return;

            Screen.SpriteBatch.DrawRectangle(RevealPerkBounds, ColorManager.Cyan, 1f, REVEAL_PERK_BOUNDS_OFFSET);
        }

        /// <summary>
        /// Pinta el tiempo restante para encontrar la ficha actual.
        /// </summary>
        void UpdateRemainingTileTime()
        {
            remainingTime = (RemainingTimeCurrentTile.ToSingle() / 1000).Round1().ToStringCulture();
            remainingTime = remainingTime.Length == 1 ? string.Concat(remainingTime, decimalSeparator, "0") : remainingTime;
            RemainingTimeLabel.Text = $"{Resource.String.REMAINING_TIME.GetString()} {remainingTime}";
        }

        /// <summary>
        /// Pinta la cantidad de fichas restantes.
        /// </summary>
        void UpdateRemainingTiles()
        {
            if (GameMode.HasUnlimitedTiles())/*En estos modos no hay límite de fichas a encontrar*/
                RemainingTilesLabel.Text = $"{Resource.String.REMAINING_TILES.GetString()} {(TilesCounter.ToString().Length == 1 ? string.Concat(" ", TilesCounter.ToString()) : TilesCounter.ToString())}/∞";
            else
                RemainingTilesLabel.Text = $"{Resource.String.REMAINING_TILES.GetString()} {(TilesCounter.ToString().Length == 1 ? string.Concat(" ", TilesCounter.ToString()) : TilesCounter.ToString())}/{GameData.TilesCurrenStage}";
        }

        /// <summary>
        /// Actualiza la etiqueta de puntos.
        /// </summary>
        void UpdatePoints(GameTime gameTime)
        {
            PointsPanel.SetValue(Points);
            PointsPanel.Update(gameTime);
        }

        /// <summary>
        /// Muestra un contador descendente previo al comienzo de la partida.
        /// </summary>
        void DrawPreCounter(GameTime gameTime)
        {
            if (lastPass == TimeSpan.Zero)
                lastPass = gameTime.TotalGameTime;

            long timeElapsed = (long)gameTime.TotalGameTime.Subtract(lastPass).TotalMilliseconds;

            if (timeElapsed > -1 && timeElapsed < 1000)/*Mostramos un fondo del color del modo*/
                return;
            else if (timeElapsed > 1000 && timeElapsed < 2000)/*Pintamos un 3*/
            {
                PlayCounterSound(0);
                FontManager.DrawString("3", counterRectangle, FontManager.GetScaleToFit("3", counterRectangle.Size.ToVector2()), ColorManager.WindowBodyColorInverted, 1, AlignHorizontal.Center);
            }
            else if (timeElapsed > 2000 && timeElapsed < 3000)/*Pintamos un 2*/
            {
                PlayCounterSound(1);
                FontManager.DrawString("2", counterRectangle, FontManager.GetScaleToFit("2", counterRectangle.Size.ToVector2()), ColorManager.WindowBodyColorInverted, 1, AlignHorizontal.Center);
            }
            else if (timeElapsed > 3000 && timeElapsed < 4000)/*Pintamos un 1*/
            {
                PlayCounterSound(2);
                FontManager.DrawString("1", counterRectangle, FontManager.GetScaleToFit("1", counterRectangle.Size.ToVector2()), ColorManager.WindowBodyColorInverted, 1, AlignHorizontal.Center);
            }
            else
                BlockBack = ShowingPreCounter = false;
        }

        /// <summary>
        /// Reproduce el sonido del contador.
        /// </summary>
        /// <param name="counterIndex"></param>
        void PlayCounterSound(int counterIndex)
        {
            if (counterPlayed[counterIndex])
                return;

            counterPlayed[counterIndex] = true;
            SoundManager.Counter.PlaySound();
        }

        #endregion
    }
}