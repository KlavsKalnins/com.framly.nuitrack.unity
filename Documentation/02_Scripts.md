# **Scripts Documentaion**

Scripts are splitted in 2 folders Editor and Runtime.<br>
Runtime scripts are organised into meaningful folders.

<br>

## **Editor**

- **HandEditor** - Button thats allow to test your hand interaction without leaving your chair

<br>

## **Runtime**

<br>

### **Behaviour**
- **InteractiveRect** - allows the ability to press/hover an image
- **SetNuitrackAvatarUser** - sets the NuitrackAvatar current user

<br>

### **Interfaces**
- **IHoverable** - abstract void Press()

<br>

### **Managers**
- **ManagerUsers** - Manages user count, user space, logic for only one user
- **UserValidator** - Validates if user is between 1 and 6 as these numbers is the user ID's

<br>

### **Tracking**
- **JointTracker** - can visualize/track any of the ~23 joints based on User ID provided by scriptable object
- **Hand** - inherits from JointTracker. should only use `nuitrack.JointType.LeftHand` and `nuitrack.JointType.RightHand`