using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsStudy
{
    public class StudyViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 연산자 입니다.
        /// </summary>
        public string Op { get; set; }
        /// <summary>
        /// 계산할 데이터 입니다. 이전입력값
        /// </summary>
        public double Op1 { get; set; }
        /// <summary>
        /// 계산할 데이터 입니다. 현재입력값
        /// </summary>
        public double Op2 { get; set; }
        /// <summary>
        /// 출력창에 입력문자를 보여주도록하는 명령입니다.
        /// </summary>
        public ICommand AddCharCommand { protected set; get; }
        /// <summary>
        /// 출력창의 문자를 하나씩 삭제하는 명령입니다.
        /// </summary>
        public ICommand DeleteCharCommand { protected set; get; }
        /// <summary>
        /// 출력창의 모든 문자를 삭제하는 명령입니다.
        /// </summary>
        public ICommand ClearCommand { protected set; get; }
        /// <summary>
        /// 연산자 입력시 처리하는 명령입니다.
        /// </summary>
        public ICommand OperationCommand { protected set; get; }
        /// <summary>
        /// 연산결과를 도출하는 명령입니다.
        /// </summary>
        public ICommand CalcCommand { protected set; get; }

        // 출력될 문자들 담아둘 변수
        string inputString = "";
        public string InputString
        {
            protected set
            {
                if (this.inputString != value)
                {
                    this.inputString = value;
                    OnPropertyChanged("InputString");
                    this.DisplayText = this.inputString;

                    //삭제 버튼을 활성화/비활성화 합니다.
                    ((Command)this.DeleteCharCommand).ChangeCanExecute();
                }
            }
            get { return this.inputString; }
        }
           
        // 출력 텍스트 박스에 대응되는 필드
        string displayText = "";
        public string DisplayText
        {
            protected set
            {
                if (this.displayText != value)
                {
                    this.displayText = value;
                    OnPropertyChanged("DisplayText");
                }
            }

            get { return this.displayText; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public StudyViewModel()
        {
            // 출력창에 입력 문자를 출력합니다.
            this.AddCharCommand = new Command<string>((Key) => { this.InputString += Key; });
            // 입력문자열이 0보다 큰 경우 마지막에서 하나씩 삭제합니다.
            this.DeleteCharCommand = new Command(
            (nothing) =>
            {
                this.InputString = this.InputString.Substring(0, this.InputString.Length - 1);
            },
            (nothing) =>
            {
                return this.InputString.Length > 0;
            }
            );

            // 초기화
            this.ClearCommand = new Command((nothing) =>
            {
                this.InputString = "";
            });

            // 연산자가 들어오면 연산자와 입력문자를 전역에 담습니다.
            this.OperationCommand = new Command<string>((key) =>
            {
                this.Op = key;
                this.Op1 = Convert.ToDouble(this.InputString);
                this.InputString = "";
            });

            // 전역에 담겨진 연산자와 입력문자열 기준으로 계산하여 출력창에 나타냅니다.
            this.CalcCommand = new Command<string>((nothing) =>
            {
                this.Op2 = Convert.ToDouble(this.InputString);

                switch (this.Op)
                {
                    case "+":  this.InputString = (this.Op1 + this.Op2).ToString(); break;
                    case "-": this.InputString = (this.Op1 - this.Op2).ToString(); break;
                    case "*": this.InputString = (this.Op1 * this.Op2).ToString(); break;
                    case "/": this.InputString = (this.Op1 / this.Op2).ToString(); break;

                }
            
            });
        }

        /// <summary>
        /// 항목변경에 따른 이벤트입니다.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
