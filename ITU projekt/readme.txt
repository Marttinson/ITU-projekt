API				(BE)	- Soubory obsahující funkce pro práci s json soubory								(xhrabo18, xrybni10)
Converters		(FE)	- Soubory pro Konvertory (View)														(xhrabo18)
Models			(BE)	- Soubory Model pro práci s daty													(xhrabo18)
Resources		(FE)	- Soubory se styly																	(xhrabo18)
Templates		(FE)	- Soubory View (User Control) obsahující definici uživatelských ovládacích prvků	(xhrabo18, xrybni10)
ViewModels		(FE)	- Soubory ViewModel obsahující logiku pro prvky uživatelského rozhraní				(xhrabo18, xrybni10)
Views			(FE)	- Soubory View hlavního okna aplikace												(xhrabo18)



xrybni10:
Api	/JsonHandler.cs
	/QuestionUtils.cs

Templates	/Choice (.xaml/.xaml.cs)
			/MemoryGame (.xaml/.xaml.cs)
			/SentenceMaking (.xaml/.xaml.cs)
			/TranslateWord (.xaml/.xaml.cs)
			/WordMatching (.xaml/.xaml.cs)

ViewModels	/ChoiceViewModel.cs
			/MemoryGameViewModel.cs
			/SentenceMakingViewModel.cs
			/TranslateWordViewModel.cs
			/WordMatchingViewModel.cs

xhrabo18:
Api			/JsonHandler.cs
Converters	/*
Models		/*
Templates	/CustomUserWordList (.xaml/.xaml.cs)
			/Graph (.xaml/.xaml.cs)
			/ListeningExercise (.xaml/.xaml.cs)
			/ReadingExercise (.xaml/.xaml.cs)
			/UnitSelection (.xaml/.xaml.cs)
ViewModels	/CustomUserWordListViewModel.cs
			/GraphViewModel.cs
			/ListeningExerciseViewModel.cs
			/ReadingExerciseViewModel.cs
			/UnitSelectionViewModel.cs
			/MainWindowViewMode.cs
Views		/MainWindowView (.xaml/.xaml.cs)
