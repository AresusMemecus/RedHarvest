using UnityEngine;
using TMPro;
using System.Collections;
using System;

public abstract class SelectableBase : MonoBehaviour
{
    private Material originalMaterial;
    [SerializeField] private Material Outlined;
    public ActionTrigger actionTrigger;

    public GameObject PopUpCloud;
    public GameObject Text;
    public GameObject Charachter;

    protected string[] ReplicaLines;
    protected TextMeshProUGUI ReplicaText;
    protected bool InteractionActive = false;
    private bool meshRendererAdded = false;
    private Material instanceMat;
    private bool isHovered = false;

    Color nonSelectable = new Color(0.5f, 0.5f, 0.5f);

    protected virtual void Awake()
    {
        if (Text != null)
        {
            ReplicaText = Text.GetComponent<TextMeshProUGUI>();
            if (ReplicaText == null)
            {
                Debug.LogError("Text GameObject не содержит компонент TextMeshProUGUI.");
            }
        }
        else
        {
            Debug.LogError("Text GameObject не присвоен в инспекторе.");
        }
    }

    void Update()
    {
        if (!isHovered || instanceMat == null || actionTrigger == null) return;

        bool inTrigger = actionTrigger.interactablesInRange.Contains(GetComponent<Collider>());
        Color targetColor = inTrigger ? Color.white : nonSelectable;
        string colorProperty = "_Color";

        Color currentColor = instanceMat.GetColor(colorProperty);
        if (currentColor != targetColor)
        {
            instanceMat.SetColor(colorProperty, targetColor);
        }
    }

    public abstract void OnSelect(); 

    public virtual void OnHoverEnter()
    {
        isHovered = true;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        var meshRenderer = GetComponent<MeshRenderer>();

        bool inTrigger = actionTrigger != null &&
                        actionTrigger.interactablesInRange.Contains(GetComponent<Collider>());

        Color targetColor = inTrigger ? Color.white : Color.red;
        string colorProperty = "Color";

        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;
            instanceMat = new Material(Outlined);
            instanceMat.SetColor(colorProperty, targetColor);
            spriteRenderer.material = instanceMat;
        }
        else if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
            instanceMat = new Material(Outlined);
            instanceMat.SetColor(colorProperty, targetColor);
            meshRenderer.materials = new Material[] { originalMaterial, instanceMat };
        }
    }


    public virtual void OnHoverExit()
    {
        isHovered = false;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        var meshRenderer = GetComponent<MeshRenderer>();

        if (spriteRenderer != null && originalMaterial != null)
        {
            spriteRenderer.material = originalMaterial;
        }
        else if (meshRenderer != null && originalMaterial != null)
        {
            meshRenderer.materials = new Material[] { originalMaterial };
        }

        instanceMat = null;
    }

    public virtual void Replica(String animation)
    {
        if (InteractionActive) return;

        bool isActive = !PopUpCloud.activeSelf;
        PopUpCloud.SetActive(isActive);
        InteractionActive = true;

        var animator = Charachter.GetComponent<Animator>();
        var characterController = Charachter.GetComponent<CharacterController>();
        var playerController = Charachter.GetComponent<PlayerController>();

        characterController.enabled = false;
        playerController.isFrozen = true;

        ReplicaText.text = ReplicaLines.Length > 0 ? ReplicaLines[0] : "…";
        animator.Play(animation);

        StartCoroutine(UnfreezeAfterSeconds());
    }

    public void Freez()
    {
        var characterController = Charachter.GetComponent<CharacterController>();
        var playerController = Charachter.GetComponent<PlayerController>();
        playerController.isFrozen = true;
        characterController.enabled = false;
    }

        public void UnFreez()
    {
        var characterController = Charachter.GetComponent<CharacterController>();
        var playerController = Charachter.GetComponent<PlayerController>();
        playerController.isFrozen = false;
        characterController.enabled = true;
    }

    protected IEnumerator UnfreezeAfterSeconds()
    {
        for (int i = 1; i < ReplicaLines.Length; i++)
        {
            yield return new WaitForSecondsRealtime(1f);
            ReplicaText.text = ReplicaLines[i];
        }

        yield return new WaitForSecondsRealtime(1f);

        var animator = Charachter.GetComponent<Animator>();
        var characterController = Charachter.GetComponent<CharacterController>();
        var playerController = Charachter.GetComponent<PlayerController>();

        animator.Play("Walk");
        PopUpCloud.SetActive(false);

        characterController.enabled = true;
        playerController.isFrozen = false;

        animator.enabled = true;
        InteractionActive = false;

        OnReplicaComplete();
    }

    public virtual void OnReplicaComplete(){}

    public void SetReplicaLines(string[] lines)
    {
        ReplicaLines = lines;
    }
}
