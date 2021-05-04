using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mood;
using System.Linq;

public class PlantIcon : MonoBehaviour
{
    private Mood.TraitValue.Scales myTrait;
    public Mood.TraitValue.Scales[] traits= new Mood.TraitValue.Scales[4];
    public Sprite[] sprites = new Sprite[4];
    SpriteRenderer mySprite;
    public Transform promotionPositon;
    bool isVisible;
    Animator myAnimator;
    float visibilityTimer;
    [SerializeField] float visibleTime = 1f;

    private Dictionary<Mood.TraitValue.Scales, Sprite> traitToSprite;
    // Start is called before the first frame update
    void Awake()
    {
        myAnimator = this.GetComponent<Animator>();
        mySprite=this.GetComponent<SpriteRenderer>();
        traitToSprite = traits.Zip(sprites, (trait, sprite) => new { trait, sprite }).ToDictionary(x => x.trait, x => x.sprite);
    }
    private void Start()
    {
        //SetTrait(Mood.TraitValue.Scales.Painful);
    }

    // Update is called once per frame
    void Update()
    {

        if (myAnimator.GetBool("isVisible"))
        {
            visibilityTimer += Time.deltaTime;
            if (visibilityTimer > visibleTime)
            {
                myAnimator.SetBool("isVisible", false);
            }
        }

    }
    public void SetTrait(Mood.TraitValue.Scales trait)
    {
        myTrait = trait;
        UpdateSprite();
    }
    private void UpdateSprite() 
    {
        //Debug.Log(traitToSprite[myTrait]);
        mySprite.sprite = traitToSprite[myTrait];
    }
    public void Appear()
    {

    }
    public void Promote()
    {
        this.transform.localPosition = promotionPositon.localPosition;
    }
    public void PopUp()
    {
        myAnimator.SetBool("isVisible", true);
        visibilityTimer = 0;
    }

}
