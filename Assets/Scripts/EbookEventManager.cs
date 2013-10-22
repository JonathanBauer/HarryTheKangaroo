using UnityEngine;
using System.Collections;

public static class EbookEventManager {

	public delegate void EbookEvent();
	
	public static event EbookEvent EbookStart, EbookPageTurn, EbookBackToMenu;
	
	public static void TriggerEbookStart() {
		// if GameStart did equal something, nothing would happen
		if(EbookStart != null){
			EbookStart();
		}
	}
	
	public static void TriggerEbookPageTurn() {
		// if GameStart did equal something, nothing would happen
		if(EbookPageTurn != null){
			EbookPageTurn();
		}
	}
	
	public static void TriggerEbookBackToMenu() {
		if(EbookBackToMenu != null){
			EbookBackToMenu();
		}
	}
}
