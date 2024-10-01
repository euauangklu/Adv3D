using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace GDD
{
    public class AnswerUI : CharacterEditor
    {
        [Header("Character Image")]
        [SerializeField] protected Image m_image;
        [SerializeField] protected List<GameObject> m_answer = new ();
        [SerializeField] private TextMeshProUGUI m_textAnswer;
        [SerializeField] private GameObject m_playAgainButton;
        [SerializeField] private GameObject m_correctTutorials;
        [SerializeField] private GameObject m_wrongTutorials;
        
        protected Image m_item;
        private QuestionTopicAsset QTA = null;
        private Sprite _shirtImage;
        private Sprite _trousersImage;
        private Sprite _scarfImage;
        private Sprite _hairImage;
        private int currentMove = 0;
        private bool isCorrect = false;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
        }

        public virtual async void OnStartUI()
        {
            QTA = GM.currentQuestionTopic;
            m_image.sprite = QTA.answerImage;
            
            await CheckAnswer(QTA, GM.characterInstance);
        }

        public async Task CheckAnswer(QuestionTopicAsset question, CharacterInstance CI)
        {
            bool isSetQuestion = false;
            if(isSetQuestion)
                SetQuestionIsAnswer();
            
            await SetClothesUnlock();
            await Task.Delay(100);
            
            currentMove = 0;
            SetAnswerToImage(0);
            
            m_textAnswer.text = $"เลือกชุด{(isCorrect ? "ถูก" : "ผิด")} ชุดนี้มีทั้งหมด {QTA.Count} ชิ้น";
        }

        private void SetAnswerToImage(int offset)
        {
            for (int i = 0; i < m_answer.Count; i++)
            {
                CanvasComponentList _canvasComponentList = m_answer[i].GetComponent<CanvasComponentList>();

                if (i < QTA.Count)
                {
                    _canvasComponentList.canvas_gameObjects[0].SetActive(true);
                    _canvasComponentList.image[0].sprite = QTA.GetCharacters[i + offset].icon;
                    _canvasComponentList.image[0].color = GetImage(QTA.GetCharacters[i + offset].clothing) != null ? Color.white : new Color(0, 0, 0, 0.5f);
                }
                else
                {
                    _canvasComponentList.canvas_gameObjects[0].SetActive(false);
                }
            }
        }

        private Sprite GetImage(string type)
        {
            switch (type)
            {
                case "Shirt":
                    return _shirtImage;
                case "Trousers":
                    return _trousersImage;
                case "Accessories_Scarf":
                    return _scarfImage;
                case "Hair":
                    return _hairImage;
                default:
                    return null;
            }
        }

        public void moveAnswer(int among)
        {
            int totalMove = QTA.Count - m_answer.Count;
            totalMove = totalMove >= 0 ? totalMove : 0;
            
            currentMove += among;
            if (currentMove >= totalMove)
            {
                currentMove = totalMove;
            }else if (currentMove <= 0)
            {
                currentMove = 0;
            }
            
            SetAnswerToImage(currentMove);
        }
        
        public bool CheckNullQuestion(CharacterAsset asset, string name)
        {
            return asset != null && asset.name == name;
        }

        public void SetQuestionIsAnswer()
        {
            QTA.isAnswer = true;
        }
        
        //Find Assets from call back
        private async Task<IEnumerable<CharacterAsset>> FindAssets(KeyValuePair<int,List<object>> asset)
        {
            return await Task.Run(() =>
            {
               return asset.Value.Select(a => a.ConvertTo<CharacterAsset>());
            });
        }

        //Reset Answer Image
        private void ResetImage()
        {
            _hairImage = null;
            _scarfImage = null;
            _trousersImage = null;
            _shirtImage = null;
        }

        //Set correct clothes to unlock;
        public async Task SetClothesUnlock()
        {
            if(QTA == null)
                return;
            
            //Reset Image Answer
            ResetImage();
            
            //Show loading
            if(m_loading != null)
                m_loading.SetActive(true);
            
            //Load Assets
            Dictionary<int, List<object>> _assets = await CDM.GetCollectAssets();
            
            //Correct And Wait Find Correct Answer
            int CorrectCount = await FindCorrectAnswer(_assets);
            
            //Check Answer
            print($"Count QTA : {QTA.Count} || Current : {CorrectCount}");
            if (CorrectCount == QTA.Count)
            {
                isCorrect = true;
                QTA.isAnswer = isCorrect;
                m_correctTutorials.SetActive(true);
            }
            else
            {
                m_wrongTutorials.SetActive(true);
            }
                
            m_playAgainButton.SetActive(!isCorrect);
            
            //Hide Loading
            if(m_loading != null)
                m_loading.SetActive(false);
        }

        //Find correct answer in character assets And return correct count
        private async Task<int> FindCorrectAnswer(Dictionary<int, List<object>> _callBackAssets)
        {
            int CorrectCount = 0;
            foreach (var asset in _callBackAssets)
            {
                //Shirt
                if (QTA.shirt != null)
                {
                    var shirt = await FindAssets(asset);
                    var shirtResult = shirt.FirstOrDefault(a => a != null && a.name == QTA.shirt.name);
                    
                    //isCorrect = CheckCorrectCondition(GM.characterInstance.shirtName, shirt);
                    if (CheckCorrectCondition("shirtName", shirtResult))
                    {
                        CorrectCount++;
                        _shirtImage = shirtResult.icon;
                    }
                }
                else
                {
                    print("QTA.shirt  is NULL.");
                    isCorrect = false;
                }

                //Trousers
                if (QTA.trousers != null)
                {
                    var trousers = await FindAssets(asset);
                    var trousersResult = trousers.FirstOrDefault(a => a != null && a.name == QTA.trousers.name);
                    //isCorrect = CheckCorrectCondition(GM.characterInstance.trousersName, trousers);
                    if (CheckCorrectCondition("trousersName", trousersResult))
                    {
                        CorrectCount++;
                        _trousersImage = trousersResult.icon;
                    }
                }
                else
                {
                    print("QTA.trousers  is NULL.");
                    isCorrect = false;
                }

                //Scarf
                if (QTA.scarf != null)
                {
                    var scarf = await FindAssets(asset);
                    var scarfResult = scarf.FirstOrDefault(a => a != null && a.name == QTA.scarf.name);
                    //isCorrect = CheckCorrectCondition(GM.characterInstance.scarfName, scarf);
                    if (CheckCorrectCondition("scarfName", scarfResult))
                    {
                        CorrectCount++;
                        _scarfImage = scarfResult.icon;
                    }
                }
                else
                {
                    print("QTA.scarf  is NULL.");
                    isCorrect = false;
                }

                //Hair
                if (QTA.hair != null)
                {
                    var hair = await FindAssets(asset);
                    var hairResult = hair.FirstOrDefault(a => a != null && a.name == QTA.hair.name);
                    //isCorrect = CheckCorrectCondition(GM.characterInstance.hairName, hair);
                    if (CheckCorrectCondition("hairName", hairResult))
                    {
                        CorrectCount++;
                        _hairImage = hairResult.icon;
                    }
                }
                else
                {
                    print("QTA.hair  is NULL.");
                    isCorrect = false;
                }
            }

            return CorrectCount;
        }

        private bool CheckCorrectCondition(String _inputEqup,CharacterAsset _SelectedAsset)
        {
            bool _result = false;
            if (_inputEqup != null && _SelectedAsset != null)
            {
                if (_inputEqup.Contains(_SelectedAsset.name))
                {
                    print($"Load 3 : {_SelectedAsset}");
                    _SelectedAsset.isUnlock = true;
                    _result = true;
                }
                else 
                {
                    print($"Not Current");
                }
            }else 
            {
                print($"Item is null");
            }
            

            return _result;
        }
        
        public void OnLoadDefault()
        {
            LoadDefaultCharacter();
        }

        public void OnResetQuestion()
        {
            GM.currentQuestionTopic = null;
        }
    }
}