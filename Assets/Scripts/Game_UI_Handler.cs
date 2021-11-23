using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_UI_Handler : MonoBehaviour {

    [Header( "Knifes Count Display" )]
    [SerializeField] GameObject _knivesPanel;
    [SerializeField] GameObject _knifeLivesIcon;
    [SerializeField] Color _usedKnifeColor;
    private int _knifeCountIndexToChange = 0;

    // ★彡[ Initially counting knives to then instantiate number of knives ]彡★
    public void  SetInitialKnifeCount( int count ) {

        for ( int i = 0; i < count + 1; i++ ) {
            
            Instantiate( _knifeLivesIcon, _knivesPanel.transform );
        }
    }

    // ★彡[ Decreasing the knife count after it is instantiated ]彡★
    public void DecreamentKnifeCount() {

        // ★彡[ Setting the panel knife icon color to usedknifecolor just to make it look faded out ]彡★
        _knivesPanel.transform.GetChild( _knifeCountIndexToChange++ ).GetComponent<Image> ().color = _usedKnifeColor;
    }

}
