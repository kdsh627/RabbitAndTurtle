using Manager;
using State.GameState;
using State.SceneState;

namespace StateMachine.SceneStateMachine
{
    public class SceneStateMachine : StateMachine
    {
        //각 상태들
        public TitleState _titleState;
        public GamePlayState _gamePlayState;
        public ClearState _clearState;

        private SceneStateManager _sceneManager;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="player"></param>
        public SceneStateMachine(SceneStateManager sceneManager)
        {
            _sceneManager = sceneManager;

            _titleState = new TitleState(sceneManager);
            _gamePlayState = new GamePlayState(sceneManager);
            _clearState = new ClearState(sceneManager);
        }

        public override void Excute()
        {
            AnyState();

            base.Excute();
        }

        private void AnyState()
        {

        }
    }
}
