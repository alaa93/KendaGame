using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using KendaGame.Helpers;

public class SpineAttachmentUI : MonoBehaviour {
  // Start is called before the first frame update

  SkeletonGraphic skeleton;


  string galssesSlot = "attachment_glasses";
  string mustachesSlot = "attachment_mustache";
  string hatsSlot = "attachment_hats";
  string hatsSlotBack = "attachment_hats_back";





  [SpineAttachment(true)]
  public string[] GlassesAttachment;

  [SpineAttachment(true)]
  public string[] MustachAttachment;

  [SpineAttachment(true)]
  public string[] HatsAttachment;

  [SpineAttachment(true)]
  public string[] HatsAttachmentBack;


  [SpineAnimation]
  public string IdleAnimation, HappyAnimation;

  public static SpineAttachmentUI instance;
  private void Awake() {
    instance = this;
  }

  private void Start() {

    skeleton = GetComponent<SkeletonGraphic>();
    int UsedHat = int.Parse(StorageHelper.ReadStorage(StorageHelper.UsedHat, "100"));
    int UsedMustache = int.Parse(StorageHelper.ReadStorage(StorageHelper.UsedMustache, "100"));
    int UsedGlasses = int.Parse(StorageHelper.ReadStorage(StorageHelper.UsedGlasses, "100"));

    if (UsedMustache != 100) skeleton.Skeleton.SetAttachment(mustachesSlot, MustachAttachment[UsedMustache]);
    if (UsedHat != 100) skeleton.Skeleton.SetAttachment(hatsSlot, HatsAttachment[UsedHat]);
    if (UsedGlasses != 100) skeleton.Skeleton.SetAttachment(galssesSlot, GlassesAttachment[UsedGlasses]);

  }


  // Update is called once per frame
  public void changeGlasses(int glassesIndex) {
    skeleton.Skeleton.SetAttachment(galssesSlot, GlassesAttachment[glassesIndex]);
    animationChange();
    

  }


  public void changeMustache(int mustacheIndex) {
    skeleton.Skeleton.SetAttachment(mustachesSlot, MustachAttachment[mustacheIndex]);
    animationChange();

  }

  public void changeHats(int hatIndex) {
    skeleton.Skeleton.SetAttachment(hatsSlot, HatsAttachment[hatIndex]);
    animationChange();
    NoHatBack();

  }

  public void changeHatsback(int hatBackIndex) {
    skeleton.Skeleton.SetAttachment(hatsSlotBack, HatsAttachmentBack[hatBackIndex]);
  }

  public void NoHatBack() {
    skeleton.Skeleton.SetAttachment(hatsSlotBack, null);
  }

  void animationChange() {
    skeleton.AnimationState.SetAnimation(0, HappyAnimation, false);
    skeleton.AnimationState.AddAnimation(0, IdleAnimation, true, 0);

  }


  public void removeAttachment() {
    skeleton.Skeleton.SetAttachment(hatsSlotBack, null);
    skeleton.Skeleton.SetAttachment(hatsSlot, null);
    skeleton.Skeleton.SetAttachment(mustachesSlot, null);
    skeleton.Skeleton.SetAttachment(galssesSlot, null);


  }
}
