using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class QuestionUI : AnswerUI
    {
        [SerializeField] private List<QuestionTopicAsset> m_questionTopics;
        [SerializeField] private UnityEvent m_onUnlockAll;
        private List<QuestionTopicAsset> _questionTopicShow;
        private Gender _charGender;
        private int _currentUnlockQuestion;

        public override void OnStartUI()
        {
            RandomQuestion();
        }
        int previousIndex = -1;
        private void RandomQuestion()
        {
            _questionTopicShow = m_questionTopics.Where(q => !q.isAnswer).ToList();

            if (_questionTopicShow.Count <= 0)
            {
                //m_onUnlockAll?.Invoke();
                //return;
                _questionTopicShow = m_questionTopics;
            }

            int index = RandomIndex();

            GM.currentQuestionTopic = _questionTopicShow[index];
            ShowQuestion();
            OnChangeGender(GM.currentQuestionTopic.gender);
        }

        int RandomIndex() {
            int index = -1;
            do {
                index = Random.Range(0, _questionTopicShow.Count);    
            }while(previousIndex == index);
            previousIndex = index;
            return index;   
        }

        public void ShowQuestion()
        {
            m_image.sprite = GM.currentQuestionTopic.questionImage;
        }
        
        public void OnChangeGender(string gender)
        {
            GM.SwitchGender(gender);
            GM.characterInstance.gender = gender;
            LoadDefaultCharacter();
        }
    }
}