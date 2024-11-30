# PBLMaker
Unityで開発(6000.0.28f1)

学籍番号 1223839　メールアドレス　c1223839@st.kanazawa-it.ac.jp　才戸淳大


Tools　→　ScriptAttachmentFinder：

・任意のスクリプトがアタッチされたオブジェクトを探せる

・Pingをクリックしたらヒエラルキーウィンドウでどこに存在するかが確認できる

・FocusをクリックしたらSceneウィンドウでどこに存在するかを確認できる


Tools　→　TaskManager：　

・タスクの難易度と優先順位を管理できるツールです

・いずれ実装しなければいけないタスクがあれば機能実装で追加してください

・バグを発見したときはバグ修正カテゴリでタスクを追加してください

ーーーーーーーーーーーーーーーーーーーコーディング規則ーーーーーーーーーーーーーーーーーーーーーーーー

1.変数名はキャメルケースを使用すること。(スペースや句読点を使わず、大文字で単語を区切って命名すること、ただし最初の1文字は小文字とする) 

〇→private int selectedIndex; 

×→private int selected_index; private int SelectedIndex;


2.関数名はパスカルケースを使用すること。(スペースや句読点を使わず、大文字で単語を区切って命名すること、ただし最初の1文字も大文字とする) 

〇→protected void OnCollisionEnter2D{}

×→protected void on_collision_enter_2D{} protected void onCollisionEnter2D{}


3.例外として、staticを使った関数/変数は全て大文字で定義、単語ごとに_(アンダーバー)で区切ること。 

〇→private static int PLAYER_SPEED;

×→private static int playerSpeed; private static int Player_Speed;


4.変数名は特定の物事や状態を表すため、名詞で定義すること。ただしboolに関しては動詞を使用してもいい 

〇→private int jumpHeight; private bool isMoving;

×→private int jump; private int walk;　※変数名に動詞を使っているため不適


5.bool変数の前にはis, can, hasなど説明や条件と対になる動詞を加えて変数が何の条件を示しているかを明確化すること。 

〇→private bool isDead; private bool canWalk; 

×→private bool Dead; private bool Walk;　※bool変数なのに条件を示した名前になっていないため不適


6.変数名/関数名は自明なものでない限り、略せずに意味のある単語にする。 

〇→private int num; private void Add(){}　　for(int i = 0; i < 10; i++){}　※number→num, for文のiは自明 

×→private int pS; private void SetPs{}　※pSが何を示しているか明示的でないため不適


7.プロトタイプを作る時はhogeなど仮置きの変数や関数を使っても構わないが、リファクタリングが済んだらきちんとした変数名にすること。

8.連続した数字を使うときは0から始めること。 

〇→Block0, Block1, Block2.... 

×→Block1, Block2, Block3....


9.関数名は動詞または動詞+名詞で命名すること。 

〇→GetDirection() FindTarget()

×→DirectionGet() TargetFind()


10.イベントを命名するときはOnを先頭に付ける 

〇→OnCollisionEnter() OnMouseButton()

×→CollisionEnter() MouseButton()


11.プロパティを命名するときはパスカルケース(関数と同様の形式)を使う

12.コメントは処理の説明なら上、定義の説明なら右に書く

//Function()の説明 private void Function() { private int a; //aの説明 }


13.1つの行に1つの宣言のみを記述する 

〇→ int x = 0; int y = 0; 

×→ int x, y;


14.関数のコメントはドキュメンテーションコメント(///)で記述する

15.publicな変数を宣言しない 

〇→ private int num; public void SetNum(int num){ this.num = num; } public int GetNum(){ return num; } 

×→public int num;


16.文字列を連結するときは文字列補間式を使う 

〇→string s = $"{hp}/{maxHp}"; 

×→string s = "hp + maxHp";


17.自明なコメントは書かない 

〇→ index++;

×→　index++; //インデックスに1を足す


18.Prefabをゲームの途中からアクティブにする場合など特例を除いてFind系の関数を使わない、使う場合はStartやAwakeの中で取得する


ーーーーーーーーーーーーーーーーーー出来る限り意識してほしいーーーーーーーーーーーーーーーーーーーーー
17.ネストを浅くする(出来る限り早期returnをして見やすいコードを心掛ける) 

〇→ private void Hoge() { if(num == 0) return; num++; }

×→ private void Hoge() { if(num != 0) { num++; } }


18.三項演算子を使う(冗長なif文は三項演算子で省略する) 

〇→ foo = i == num ? 0 : 1;

×→ if(i == num) { foo = 0; } else { foo = 1; }
